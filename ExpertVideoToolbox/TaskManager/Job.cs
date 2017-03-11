using MediaInfoLib;
using ExpertVideoToolbox.cmdCodeGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExpertVideoToolbox.TaskManager;
using Newtonsoft.Json;

namespace ExpertVideoToolbox.taskManager
{
    class Job
    {
        public delegate void ErrorHandler();          //声明委托
        public event ErrorHandler ErrorEvent;         //声明事件

        delegate void InnerMethodDelagate();           
        
        const int AUDIOENCODE = 0;
        const int VIDEOENCODE = 1;
        const int MUXER = 2;
        const int AUDIOCOPY = 3;
        const int DELETEVIDEOTEMP = 4;
        const int DELETEAUDIOTEMP = 5;

        const int ONLYVIDEO = 0;
        const int SUPPRESSAUDIO = 1;
        const int COPYAUDIO = 2;

        long outPutCmdCount = 0;
        long outPutCmdCheckCount = 0;
       
        private runningPool rp;
        public videoTask[] tasks;
        public int num;
        public int finishedNum;
        public int threadsNum;
        private bool[] isfinished;
        private string outputFolderPath;

        private RunningStatusForm rsForm;

        private int[] PIDs;
        private int subTaskPID;

        private int startTime = 0;
        private int endTime;

        private int checkPattern;
        private int[] checkTime;
        private int[] checkFrame;
        private double[] fps;

        private bool audioProcessing;
        private bool encoding;
        private bool postProcessing;
        private bool muxing;
        private bool isfailed;

        private StringBuilder log;
        private string miniLog;
        private int reportCount;

        private Thread ctrlThread;
        Thread[] threads;

        //private Process process = null;

        public Job(filePathStorage tempQueue)
        {
            // 从storage读取num
            this.num = tempQueue.Count();
            this.threadsNum = 1;

            this.finishedNum = 0;

            encoding = false;
            audioProcessing = false;
            muxing = false;
            
            // 初始化task数组来feed下面的queue
            this.tasks = new videoTask[num];
            this.PIDs = new int[threadsNum];
            this.subTaskPID = Int32.MaxValue;
            this.isfinished = new bool[threadsNum];

            reportCount = 0;
            checkPattern = 80;
            int cpx2 = checkPattern + checkPattern;
            checkTime = new int[cpx2];
            checkFrame = new int[cpx2];
            this.fps = new double[cpx2];
            for (int i = 0; i < cpx2; i++)
            {
                this.fps[i] = 0;
            }

            for (int i = 0; i < num; i++ )
            {
                this.tasks[i] = tempQueue.get(i);          
            }

            // 初始化运行池pool
            this.rp = new runningPool(this.num, this.tasks);  // 运行池持有队列，需传递队列的相关参数

            // 初始化运行状态窗口
            this.rsForm  = new RunningStatusForm();

            log = new StringBuilder("");
            this.rsForm.AbortEvent += saveLog2File;
            this.rsForm.ClosingEvent += AllThreadKill;
 
        }
        
        public void runJob(string ofp)
        {
            this.rsForm.Show();
            this.rsForm.setStatusBarFilesCountLabel(this.finishedNum, this.num);

            this.outputFolderPath = ofp;

            this.threads = new Thread[this.threadsNum];
            for (int i = 0; i < threadsNum; i++)
            {
                int p = i;
                Thread th = new Thread(()=>
                {
                    this.isfinished[p] = false;
                    this.threadRun(p);
                });
                threads[p] = th;
            }
            for (int i = 0; i < threadsNum; i++)
            {
                threads[i].Start();
            }
           
        }

        public int getPID()
        {
            return this.PIDs[0];
        }

        private void threadRun(int threadIndex)
        {
            videoTask t = rp.getTask();
            videoTask next;
            
            for (int i = 0; i < this.num; i++)
            {
                this.isfailed = false;                  // 将任务失败状态设为否
                if (i != 0)
                {
                    rp.removeTask(t, false);            // false表示成功完成编码没有失败
                }
 
                this.runTask(t, this.rsForm.process);

                next = rp.getTask();                    // get之后任务已从运行池的等待队列中pop出了，无需再次get
                //MessageBox.Show(next.printTask());
                if (next == null)
                {
                    rp.removeTask(t, false); 
                    break;
                }        
                t = next;     
            }           
        }

        private void runTask(videoTask t, Process process)
        {
            this.rsForm.setPercent("0.0");
            this.rsForm.setTime("");
            this.rsForm.setFps("");
            this.rsForm.setEta("");
            
            string fp = t.getFP();

            process = new System.Diagnostics.Process();

            process.StartInfo.FileName = "cmd";

            // 必须禁用操作系统外壳程序  
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;

            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
           
            process.Start();

            this.PIDs[0] = process.Id;
            this.rsForm.PID = process.Id;
            this.rsForm.setStopBtnState(true);
           
            // 找到预设存储的文件并提取到taskSetting中
            string tsfp = System.Windows.Forms.Application.StartupPath + "\\taskSettings\\" + t.getSetting() + ".json";
            taskSetting ts = JsonConvert.DeserializeObject<taskSetting>(File.ReadAllText(tsfp));

            // 将encoder信息传给信息显示界面
            this.rsForm.encodingProgram = ts.encoder;          
            this.rsForm.HideVideoEncoderSetting();

            cmdCode c = new cmdCode(fp, ts, this.outputFolderPath);
            string cmd;

            int type = c.taskType();
            int checkNum = 0;

            int beforeProcessCheckTime = 2000;
            int processCheckInterval = 1000;
            const int checkF = 20;

            // 定义一个内部（匿名）方法
            // 整个视频转换过程结束时
            InnerMethodDelagate afterSuccess = delegate()
            {
                this.finishedNum++;
                this.rsForm.setStatusBarFilesCountLabel(this.finishedNum, this.num);

                process.CancelErrorRead();
                process.CancelOutputRead();

                this.rsForm.setEta("");
                this.rsForm.setFps("");
                this.rsForm.setTime("");
                this.rsForm.SetTaskStepsLabel(true);

                Process p = Process.GetProcessById(this.PIDs[0]);
                p.Kill();
                this.rsForm.setStopBtnState(false);
                this.rsForm.setPercent("-3");
                this.isfinished[0] = true;
                this.reportCount = 0;
                this.log.Clear();

                miniLog += t.getFP() + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                saveMiniLog2File();
            };
            
            // 运行失败时调用
            InnerMethodDelagate afterFailed = delegate()
            {
                this.isfailed = true;
                
                saveLog2File();

                this.rsForm.setPercent("-1");
                this.rsForm.setTime("发生错误");
                this.rsForm.setFps("日志保存在程序目录");
                
                int sleepTime = 5;  // 设置失败后继续下一个任务的时间
                this.rsForm.setEta(sleepTime.ToString() + "秒后继续运行");

                this.rsForm.setStatusBarLabelTextColorRED();
                this.finishedNum++;
                this.rsForm.setStatusBarFilesCountLabel(this.finishedNum, this.num);
                this.rsForm.HideVideoEncoderSetting();

                process.CancelErrorRead();
                process.CancelOutputRead();
                
                Thread.Sleep(sleepTime * 1000);

                Process p = Process.GetProcessById(this.PIDs[0]);
                p.Kill();
                this.rsForm.setStopBtnState(false);
                this.rsForm.setPercent("-3");
                this.isfinished[0] = true;
                this.reportCount = 0;
                this.log.Clear();
            };
            
            InnerMethodDelagate VideoEncode = delegate()
            {
                // 视频编码                                             
                this.encoding = true;
                this.rsForm.setPercent("0.0");
                             
                cmd = c.cmdCodeGenerate(VIDEOENCODE);
                process.StandardInput.WriteLine(cmd);
               
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                checkNum = 0;
                this.reportCount = 0;
                int cpx2 = this.checkPattern + this.checkPattern;
                for (int i = 0; i < cpx2; i++)
                {
                    checkFrame[i] = 0;
                }
                for (int i = 0; i < cpx2; i++)
                {
                    this.fps[i] = 0;
                }

                Thread.Sleep(beforeProcessCheckTime);
                
                this.rsForm.ShowVideoEncoderSetting();

                Process p = GetSubTaskProcess(ts.encoder);
                if (p != null)
                {
                    this.subTaskPID = p.Id;
                } else
                {
                    this.subTaskPID = -1;
                }

                while (this.encoding == true || this.postProcessing == true)
                {
                    try
                    {
                        Process.GetProcessById(this.subTaskPID);
                    }
                    catch (Exception e)
                    {
                        if (ConfirmFailed())
                        {
                            afterFailed();
                        }                       
                        return;
                    }
                   
                    Thread.Sleep(processCheckInterval);
                }
                process.CancelErrorRead();
                process.CancelOutputRead();
                this.rsForm.HideVideoEncoderSetting();
            };

            InnerMethodDelagate Mux = delegate()
            {
                // muxer
                this.muxing = true;
                this.rsForm.SetTaskStepsLabel(false, 3, 3, MUXER);

                cmd = c.cmdCodeGenerate(MUXER);
                process.StandardInput.WriteLine(cmd);
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                checkNum = 0;

                Thread.Sleep(beforeProcessCheckTime);  // 有些超短的视频（1-2M），如果不加这句就会直接判定为任务已失败，疑似原因：没等判断完进程就已经结束

                string muxerProcessName = "";
                if (String.Equals("mp4", ts.outputFormat))
                {
                    muxerProcessName = "mp4Box";
                }
                else if (String.Equals("mkv", ts.outputFormat))
                {
                    muxerProcessName = "mkvmerge";
                }

                Process p = GetSubTaskProcess(muxerProcessName);
                if (p != null)
                {
                    this.subTaskPID = p.Id;
                }
                else
                {
                    this.subTaskPID = -1;
                }
               
                while (this.muxing == true)
                {
                    try
                    {
                        Process.GetProcessById(this.subTaskPID);
                    }
                    catch (Exception e)
                    {
                        afterFailed();
                        return;
                    }
                    
                    Thread.Sleep(processCheckInterval);
                    //checkNum = checkCmdRunning(checkNum, checkF);
                    //if (checkNum == -1)
                    //{
                    //    return;
                    //}
                }
                afterSuccess();

                string tempVideoFp = c.cmdCodeGenerate(DELETEVIDEOTEMP);
                string tempAudioFp = c.cmdCodeGenerate(DELETEAUDIOTEMP);
                try
                {
                    File.Delete(tempVideoFp);
                    File.Delete(tempAudioFp);
                }
                catch (System.IO.IOException ex)
                {
                    this.log.AppendLine("出现异常：" + ex);
                    saveLog2File();
                }
            };

            // 音频编码或复制开始前更新UI(显示音频总时长)
            InnerMethodDelagate DispAudioDuration = delegate()
            {
                // MediaInfo读取音频时长
                MediaInfo MI = new MediaInfo();
                string duration;
                MI.Open(t.getFP());
                duration = MI.Get(StreamKind.Audio, 0, 69);

                if (!String.IsNullOrWhiteSpace(duration))
                {
                    this.rsForm.setTime("0/" + duration);
                }

                this.rsForm.setPercent("0.0", AUDIOENCODE);
            };

            InnerMethodDelagate ClearUIAfterAudioProcessing = delegate()
            {
                this.rsForm.setPercent("0.0");
                this.rsForm.setTime("");
                this.rsForm.setFps("");
            };

            switch (type)
            {
                case ONLYVIDEO:

                    this.rsForm.SetTaskStepsLabel(false, 1, 1, VIDEOENCODE);

                    VideoEncode();

                    // 一个子任务失败了意味着这个文件的编码任务失败，所以在所有子任务开始时都要检查checkNum是否为-1
                    if (isfailed)
                    {
                        return;
                    }                
                    afterSuccess();

                    break;
                case COPYAUDIO:
                    // 复制音频
                    cmd = c.cmdCodeGenerate(AUDIOCOPY);
                    process.StandardInput.WriteLine(cmd);
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();
                    this.audioProcessing = true;
                    this.rsForm.SetTaskStepsLabel(false, 1, 3, AUDIOCOPY);
                    DispAudioDuration();
                    
                    checkNum = 0;

                    Thread.Sleep(beforeProcessCheckTime);

                    Process p = GetSubTaskProcess("ffmpeg");
                    if (p != null)
                    {
                        this.subTaskPID = p.Id;
                    }
                    else
                    {
                        this.subTaskPID = -1;
                    }

                    while (this.audioProcessing == true)
                    {
                        try
                        {
                            Process.GetProcessById(this.subTaskPID);
                        }
                        catch (Exception e)
                        {
                            afterFailed();
                            return;
                        }

                        Thread.Sleep(processCheckInterval);
                        //checkNum = checkCmdRunning(checkNum, checkF);
                        //if (checkNum == -1)
                        //{
                        //    return;
                        //}
                    }
                    ClearUIAfterAudioProcessing();

                    process.CancelErrorRead();
                    process.CancelOutputRead();

                    this.rsForm.SetTaskStepsLabel(false, 2, 3, VIDEOENCODE);

                    // 一个子任务失败了意味着这个文件的编码任务失败，所以在所有子任务开始时都要检查checkNum是否为-1
                    if (isfailed)
                    {
                        return;
                    }               
                    VideoEncode();

                    // 一个子任务失败了意味着这个文件的编码任务失败，所以在所有子任务开始时都要检查checkNum是否为-1
                    if (isfailed)
                    {
                        return;
                    }               
                    Mux();

                    break;
                case SUPPRESSAUDIO:
                    // 音频编码
                    cmd = c.cmdCodeGenerate(AUDIOENCODE);
                    process.StandardInput.WriteLine(cmd);
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();
                    this.audioProcessing = true;
                    this.rsForm.SetTaskStepsLabel(false, 1, 3, AUDIOENCODE);
                    DispAudioDuration();

                    checkNum = 0;

                    Thread.Sleep(beforeProcessCheckTime);
                
                    Process p2 = GetSubTaskProcess("ffmpeg");
                    if (p2 != null)
                    {
                        this.subTaskPID = p2.Id;
                    }
                    else
                    {
                        this.subTaskPID = -1;
                    }

                    while (this.audioProcessing == true)
                    {
                        try
                        {
                            Process.GetProcessById(this.subTaskPID);
                        }
                        catch (Exception e)
                        {
                            afterFailed();
                            return;
                        }

                        Thread.Sleep(processCheckInterval);
                        //checkNum = checkCmdRunning(checkNum, checkF);
                        //if (checkNum == -1)
                        //{
                        //    return;
                        //}
                    }
                    ClearUIAfterAudioProcessing();

                    process.CancelErrorRead();
                    process.CancelOutputRead();

                    this.rsForm.SetTaskStepsLabel(false, 2, 3, VIDEOENCODE);

                    // 一个子任务失败了意味着这个文件的编码任务失败，所以在所有子任务开始时都要检查checkNum是否为-1
                    if (isfailed)
                    {
                        return;
                    }               
                    VideoEncode();

                    // 一个子任务失败了意味着这个文件的编码任务失败，所以在所有子任务开始时都要检查checkNum是否为-1
                    if (isfailed)
                    {
                        return;
                    }              
                    Mux();

                    break;
                default:
                    cmd = "";
                    break;
            }

            //MessageBox.Show(cmd);           
        }

        private void ResetRsForm()
        {
            this.rsForm.setEta("");
            this.rsForm.setFps("");
            this.rsForm.setTime("");

            this.rsForm.setStatusBarFilesCountLabel(this.finishedNum, this.num);
        }        

        public void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            this.outPutCmdCount++;
            if (!String.IsNullOrEmpty(outLine.Data) && this.num != this.finishedNum)
            {               
                string o = outLine.Data.ToString();
                log.AppendLine(o);               

                // 音频编码解析
                if (this.audioProcessing == true)
                {
                    string audioCopyFinishFlag = "video:0kB audio:";
                    string audioEncodeFinishFlag = "Optimizing...done";
                    this.rsForm.setPercent("-1");

                    string time = "";
                    string totalTime = "";

                    InnerMethodDelagate CalculateAndDispProgress = delegate()
                    {
                        string[] splitTemp = totalTime.Split(new char[] { ':' });

                        string HH = splitTemp[0];
                        string MM = splitTemp[1];
                        string SSMMM = splitTemp[2];

                        int hr = Convert.ToInt32(HH);
                        int min = Convert.ToInt32(MM);
                        double sec = Convert.ToDouble(SSMMM);
                        double totalTimeValue = hr * 3600 + min * 60 + sec;

                        splitTemp = time.Split(new char[] { ':' });
                        if (splitTemp.Length == 2)
                        {
                            HH = "00";
                            MM = splitTemp[0];
                            SSMMM = splitTemp[1];
                        }
                        else if (splitTemp.Length == 3)
                        {
                            HH = splitTemp[0];
                            MM = splitTemp[1];
                            SSMMM = splitTemp[2];
                        }
                        hr = Convert.ToInt32(HH);
                        min = Convert.ToInt32(MM);
                        sec = Convert.ToDouble(SSMMM);
                        double currentTimeValue = hr * 3600 + min * 60 + sec;
                        double percent = currentTimeValue / totalTimeValue;
                        this.rsForm.setPercent(percent.ToString("0.0"), AUDIOENCODE);  //这一模式需要保留一位小数
                    };

                    if (o.Contains(audioCopyFinishFlag) || o.Contains(audioEncodeFinishFlag))
                    {
                        this.audioProcessing = false;
                    }
                    else
                    {
                        if (o.Contains("time=")) // 表示是音频复制模式
                        {
                            int timeIndex = o.IndexOf("time=") + 5;
                            int timeLength = 11;
                            time = o.Substring(timeIndex, timeLength);

                            string currentTimeText = this.rsForm.getCurrentTimeText();
                            int splitIndex = currentTimeText.IndexOf("/");
                            totalTime = currentTimeText.Substring(splitIndex + 1, currentTimeText.Length - splitIndex - 1);
                            if (String.IsNullOrWhiteSpace(totalTime))
                            {
                                totalTime = time;
                            }
                            this.rsForm.setTime(time + "/" + totalTime);
                            CalculateAndDispProgress();
                        }
                        else  // 音频编码模式
                        {
                            // 通过ffmpeg读取送到qaac转码
                            // o 的格式 = 时间(速度x) 特征是x)
                            int endIndex = o.IndexOf("x)");
                            int startIndex = o.IndexOf("(");
                            // 速度 xxx.x 最多5位字符，间隔不应大于5
                            if (startIndex != -1 && endIndex != -1 && endIndex - startIndex < 6 && endIndex - startIndex > 0)
                            {
                                string speed = o.Substring(startIndex + 1, endIndex - startIndex - 1);  // 第二个参数是子串的长度
                                this.rsForm.setFps(speed + "x");

                                // 从UI获取音频时长
                                string lastCurrentTimeText = this.rsForm.getCurrentTimeText();
                                int totalTimeStartIndex = lastCurrentTimeText.IndexOf("/") + 1;
                                int totalTimeLength = lastCurrentTimeText.Length - totalTimeStartIndex;
                                totalTime = lastCurrentTimeText.Substring(totalTimeStartIndex, totalTimeLength);

                                time = o.Substring(0, startIndex - 1);
                                
                                if (String.IsNullOrWhiteSpace(totalTime))
                                {
                                    totalTime = time;
                                }
                                this.rsForm.setTime(time + "/" + totalTime);

                                CalculateAndDispProgress();
                            }
                        }                    
                    }
                    return;
                }

                // postProcessing解析
                if (this.postProcessing == true)
                {     
                    string ppfinishFlag01 = "encoded ";
                    string ppfinishFlag02 = " frames";
                    this.rsForm.setPercent("-2");
                    if (o.Contains(ppfinishFlag01) && o.Contains(ppfinishFlag02))
                    {                        
                        this.postProcessing = false;
                        miniLog += o + Environment.NewLine;
                    }
                }

                // muxer解析
                if (this.muxing == true)
                {
                    string muxingFinishFlag_mp401 = "ISO File Writing: ";
                    string muxingFinishFlag_mp402 = "| (99/100)";
                    string muxingFinishFlag_mkv = "Muxing took ";
                    this.rsForm.setPercent("-2");
                    if ((o.Contains(muxingFinishFlag_mp401) && o.Contains(muxingFinishFlag_mp402))
                        || o.Contains(muxingFinishFlag_mkv))
                    {
                        this.muxing = false;
                    }
                    return;
                }

                // 视频编码解析
                string finishFlag01 = "x264 [info]: kb/s:";
                string finishFlag02 = "x265 [info]: consecutive B-frames:";
                //string finishFlag02 = " frames";
                
                if (this.encoding)
                {
                    if (this.startTime == 0)  // 仅在开始时运行，编码结束后要重置为0
                    {
                        this.startTime = System.Environment.TickCount;
                    }

                    if (o.Contains(finishFlag01) || o.Contains(finishFlag02))  // 视频编码结束了
                    {
                        miniLog += o + Environment.NewLine;

                        this.endTime = System.Environment.TickCount;

                        int timeElapsed = this.endTime - this.startTime;

                        this.startTime = 0;

                        this.rsForm.setPercent("100");
                        this.rsForm.setFps("");
                        this.rsForm.setEta("");
                        this.rsForm.setTime("");

                        // 视频编码结束后，x264可能先对视频流单独混流一次
                        this.postProcessing = true;
                        this.encoding = false;
                    }
                    else  // 编码未结束更新进度
                    {
                        if (!o.Contains("indexing input file") && !o.Contains("[info]"))
                        {
                            // o 的格式 = [xx.x%] 已完成帧数/总帧数, ...
                            int endIndex = o.IndexOf("%]");
                            int startIndex = o.IndexOf("[");
                            // 进度 xx.x 最多4位字符，间隔不应大于4 (index之间差值不得大于5)
                            if (startIndex != -1 && endIndex != -1 && endIndex - startIndex < 6 && endIndex - startIndex > 0)
                            {
                                string progress = o.Substring(startIndex + 1, endIndex - startIndex - 1);  // 第二个参数是子串的长度
                                this.rsForm.setPercent(progress);
                                if (Convert.ToDouble(progress) >= 99.9)
                                {
                                    miniLog += o + Environment.NewLine;
                                }
                            }

                            int checkIndex = this.reportCount % (checkPattern * 2);
                            checkTime[checkIndex] = System.Environment.TickCount;

                            // 从cmd中读取已经编码的帧数
                            int framesIndex = o.IndexOf(" frames");

                            string encodedFrames = "";
                            string totalFrames = "";

                            if (framesIndex != -1)
                            {
                                // frames 格式： xxx/xxxxx (a/b, a的位数小于等于b的位数)
                                string frames = o.Substring(endIndex + 3, framesIndex - endIndex - 3);
                                this.rsForm.setTime(frames);

                                string[] splitTemp = frames.Split(new char[] { '/' });
                                encodedFrames = splitTemp[0];
                                totalFrames = splitTemp[1];

                                checkFrame[checkIndex] = Convert.ToInt32(encodedFrames);
                            }
                            if (this.reportCount >= checkPattern)
                            {
                                // 定义一个内部（匿名）方法
                                // 分析并计算fps和ETA
                                InnerMethodDelagate processFpsAndETA = delegate()
                                {
                                    // 取两位小数的方法
                                    System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                                    nfi.NumberDecimalDigits = 2;
                                    this.rsForm.setFps(fps[checkIndex].ToString("N", nfi) + " fps");

                                    // 计算优化后的ETA
                                    double avgFps = NonZeroAverage(this.fps);
                                    double eta = (double)(Convert.ToInt32(totalFrames) - checkFrame[checkIndex]) / avgFps / 60.0;
                                 
                                    double i = Math.Floor(eta);  // 去除小数部分
                                    double d = eta - i;

                                    double sec = d * 60.0;

                                    d = i / 60.0;
                                    double h = Math.Floor(d);
                                    double min = i - h * 60;

                                    string etaStr = ((int)h).ToString("00") + ":" + ((int)min).ToString("00") + ":" + ((int)sec).ToString("00");

                                    this.rsForm.setEta(etaStr);
                                };

                                if (checkIndex >= checkPattern)
                                {
                                    // 注意：下两句和另一部分是不一样的，不能复制
                                    int timeIntetval = checkTime[checkIndex] - checkTime[checkIndex - checkPattern];
                                    int framesInteval = checkFrame[checkIndex] - checkFrame[checkIndex - checkPattern];
                                    fps[checkIndex] = (double)framesInteval / (double)timeIntetval * 1000;

                                    if (fps[checkIndex] > 0)
                                    {
                                        processFpsAndETA();
                                    }
                                }
                                else
                                {
                                    // 注意：下两句和另一部分是不一样的，不能复制
                                    int timeIntetval = checkTime[checkIndex] - checkTime[checkIndex + checkPattern];
                                    int framesInteval = checkFrame[checkIndex] - checkFrame[checkIndex + checkPattern];
                                    fps[checkIndex] = (double)framesInteval / (double)timeIntetval * 1000;

                                    if (fps[checkIndex] > 0)
                                    {
                                        processFpsAndETA();
                                    }
                                }
                            }
                            else
                            {
                                this.rsForm.setEta("正在计算...");
                                this.rsForm.setFps("正在统计...");
                            }

                            this.reportCount++;
                        }
                        else
                        {
                            miniLog += o + Environment.NewLine;
                            this.rsForm.setPercent("0.0");
                            this.rsForm.setTime("索引中");
                        }
                    }
                }
            }
        }
  
        private string convertToTimerFormat(string s)
        {
            if (s.Length == 1)
            {
                return "0" + s;
            } else
            {
                return s;
            }
        }

        private void AllThreadKill()
        {
            for (int i = 0; i < threadsNum; i++)
            {
                threads[i].Abort();
            }
        }

        private void saveLog2File()
        {
            string dateAndTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fp = System.Windows.Forms.Application.StartupPath + "\\" + "EVTLog_"
                + dateAndTime + ".log";
            System.IO.File.WriteAllText(fp, this.log.ToString(), Encoding.UTF8);
        }

        private void saveMiniLog2File()
        {
            string fp = System.Windows.Forms.Application.StartupPath + "\\" + "EVTminiLog" + ".log";
            System.IO.File.AppendAllText(fp, this.miniLog, Encoding.UTF8);
        }

        // 成功完成一个任务时才会调用的debug方法
        private void debugLog2File()
        {
            string dateAndTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fp = System.Windows.Forms.Application.StartupPath + "\\" + "EVTLog_"
                + dateAndTime + ".log";
            System.IO.File.WriteAllText(fp, this.log.ToString(), Encoding.UTF8);
        }

        private int checkCmdRunning(int t, int _t)
        {
            // 判断_t次循环内有没有新的命令行信息输出
            t = (++t == _t) ? t - _t : t;
            if (t == 0 && this.outPutCmdCount == this.outPutCmdCheckCount)  // && (this.encoding || this.muxing || this.audioProcessing)
            {                
                saveLog2File();

                this.rsForm.setPercent("-1");
                this.rsForm.setTime("发生错误");
                this.rsForm.setFps("日志保存在程序目录");
                
                int sleepTime = 5;  // 设置失败后继续下一个任务的时间
                this.rsForm.setEta(sleepTime.ToString() + "秒后继续运行");

                t = -1;
                this.rsForm.setStatusBarLabelTextColorRED();
                this.finishedNum++;
                this.rsForm.setStatusBarFilesCountLabel(this.finishedNum, this.num);
                
                Thread.Sleep(sleepTime * 1000);

                Process p = Process.GetProcessById(this.PIDs[0]);
                p.Kill();
                this.rsForm.setStopBtnState(false);
                this.rsForm.setPercent("-3");
                this.isfinished[0] = true;
                this.reportCount = 0;
                this.log.Clear();
            }
            else if (t == 0)
            {
                this.outPutCmdCheckCount = this.outPutCmdCount;
            }
            return t;
        }

        // 新版的检查子任务是否正常运行的方法(Not used)
        private bool checkCmdRunning(string processName)
        {
            Process[] ps = Process.GetProcessesByName(changeFileExtName(processName));  // 必须输入不含.exe的进程名

            Process p = null;

            for (int i = 0; i < ps.Length; i++)
            {
                Process parent = ExpertVideoToolbox.ProcessControl.ParentProcessUtilities.GetParentProcess(ps[i].Id);
                if (parent != null && parent.Id == this.getPID())
                {
                    p = ps[i];
                    break;
                }
            }
               
            if (p == null)  // p为null表示要么没进入上述循环，要么ps为空
            {               
                return false;
            }
            return true;
        }

        private bool ConfirmFailed()
        {
            Thread.Sleep(1000);
            if (this.postProcessing == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // 得到本程序子任务对应的进程
        private Process GetSubTaskProcess(string processName)
        {
            Process[] ps = Process.GetProcessesByName(changeFileExtName(processName));  // 必须输入不含.exe的进程名

            Process p = null;

            for (int i = 0; i < ps.Length; i++)
            {
                Process parent = ExpertVideoToolbox.ProcessControl.ParentProcessUtilities.GetParentProcess(ps[i].Id);
                if (parent != null && parent.Id == this.getPID())
                {
                    p = ps[i];
                    break;
                }
            }
            return p;
        }

        // 求剔除零之后数组的平均值
        private double NonZeroAverage(double[] d)
        {
            int count = d.Length;
            int nonZeroCount = 0;
            double sum = 0.0;
            for (int i = 0; i < count; i++)
            {
                if (d[i] != 0)
                {
                    nonZeroCount++;
                    sum += d[i];
                }
            }
            return sum / (double)nonZeroCount;
        }

        // 已知问题：如果输入为无扩展名的文件名而且其路径中含有.，就会替换最后一个点后所有字符
        private string changeFileExtName(string originFp, string ext = "")
        {
            if (!originFp.Contains('.'))
            {
                if (!String.IsNullOrWhiteSpace(ext))
                {
                    return originFp + "." + ext;
                }
                else
                {
                    return originFp;
                }
            }

            string[] s = originFp.Split(new char[] { '.' });
            int length = s.Length;
            s[length - 1] = ext;
            string finalFileName = "";
            for (int i = 0; i < length; i++)
            {
                finalFileName += s[i];
                if (i != length - 1)
                {
                    finalFileName += ".";
                }
            }
            if (string.IsNullOrWhiteSpace(ext)) // 当需要的扩展名为空时，去除字符串最后位置的一个点
            {
                finalFileName = finalFileName.Substring(0, finalFileName.Length - 1);
            }
            return finalFileName;
        }
    }
}
