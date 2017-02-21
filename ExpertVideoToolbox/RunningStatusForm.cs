using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertVideoToolbox
{
    public partial class RunningStatusForm : Form
    {
        const int IDLE = 64;
        const int BELOWNORMAL = 16384;
        const int NORMAL = 32;
        const int ABOVENORMAL = 32768;
        const int HIGHPRIORITY = 128;
        const int REALTIME = 256;
        
        delegate void SetTextCallback(string commandLine);

        public delegate void AbortHandler();           //声明委托
        public event AbortHandler AbortEvent;          //声明事件

        public delegate void ClosingHandler();           //声明委托
        public event ClosingHandler ClosingEvent;        //声明事件
        public Process process = null;

        public int PID;

        const int AUDIOENCODE = 0;
        const int VIDEOENCODE = 1;
        const int MUXER = 2;
        const int AUDIOCOPY = 3;

        public string encodingProgram = "";

        public ProcessControl processControl;

        public RunningStatusForm()
        {
            this.processControl = new ProcessControl();
            InitializeComponent();
        }

        public void setPercent(string percent, int mode = VIDEOENCODE)
        {
            if (!percent.Contains("-")) 
            {
                if (mode == VIDEOENCODE)
                {
                    double videoEncodePst = Convert.ToDouble(percent) / 100.0;
                    double globelPst100x = 1.0 + 98.0 * videoEncodePst;
                    this.progress.Value = (int)globelPst100x;
                    this.progressLabel.Text = globelPst100x.ToString("0.0") + "%";
                }
                else
                {
                    this.progressLabel.Text = percent + "%";  // 这一模式下传进来的字符串已经保留一位小数
                }

            }
            else
            {
                if (String.Equals(percent, "-1"))
                {
                    this.progress.Value = 0;
                } else if (String.Equals(percent, "-2"))
                {
                    this.progress.Value = 99;
                } else
                {
                    this.progress.Value = 100;
                }
                this.progressLabel.Text = "";
            }
        }

        public void setTime(string time)
        {
            this.currentPostionDataTB.Text = time;
            this.currentPostionDataTB.Select(this.currentPostionDataTB.TextLength, 0);
        }

        public void setFps(string fps)
        {
            this.fpsDataTB.Text = fps;
        }

        public void setEta(string eta)
        {
            this.estETADataTB.Text = eta;
        }

        private void RunningStatusForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (process != null)
            {
                process.Close();
            }
        }
        public void setStopBtnState(bool enabled)
        {
            this.stopBtn.Enabled = enabled;
        }

        public string getCurrentTimeText()
        {
            return this.currentPostionDataTB.Text;
        }

        public void setWindowTitle(string title)
        {
            this.Text = title;
        }

        public void setStatusBarFilesCountLabel(int finished, int all)
        {
            string suffix = this.filesCountLabel.ForeColor == Color.Red ? " (存在失败任务)  请查询日志" : "";
            string s = finished.ToString() + "/" + all.ToString() + suffix;
            this.filesCountLabel.Text = s;
        }

        public void setStatusBarLabelTextColorRED()
        {
            this.filesCountLabel.ForeColor = Color.Red;
        }

        public void SetTaskStepsLabel(string tss)
        {
            this.taskStepsLabel.Text = tss;
        }
        
        public void SetTaskStepsLabel(bool finish, int currentStepIndex = 0, int totalStepsCount = 0, int mode = 0)
        {
            string tss = "";
            if (finish)
            {
                tss = "已完成";
            }
            else
            {
                tss = "正在进行：步骤 " + currentStepIndex.ToString() + "/" + totalStepsCount.ToString() + " - ";
                string modeName = "";
                switch (mode)
                {
                    case AUDIOENCODE:
                        modeName = "音频编码";
                        break;
                    case AUDIOCOPY:
                        modeName = "复制音频";
                        break;
                    case VIDEOENCODE:
                        modeName = "视频编码";
                        break;
                    case MUXER:
                        modeName = "封装";
                        break;
                    default:
                        break;
                }
                tss += modeName;
            }
            
            this.SetTaskStepsLabel(tss);       
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要中止转换吗？", "确认中止", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // 点确定的代码
                // 传入字符串不可加.exe
                //Process[] p = Process.GetProcessesByName(this.encodingProgram.Substring(0, this.encodingProgram.Length - 4));
                
                //int count = p.Length;
                //for (int i = 0; i < count; i++)
                //{
                //    p[i].Kill();
                //}
                //this.stopBtn.Enabled = false;

                KillProcessAndChildren(PID);

                string stopWindowTitle = "用户中止操作";
                this.setWindowTitle(stopWindowTitle);

                AbortEvent();
                ClosingEvent();

                this.stopBtn.Enabled = false;
            }
            else
            {
                //点取消的代码 
            }
        }

        /** 
        * 传入参数：父进程id 
        * 功能：根据父进程id，杀死与之相关的进程树 
        */
        private static void KillProcessAndChildren(int pid)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                Console.WriteLine(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                /* process already exited */
            }
        } 
        private void RunningStatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosingEvent();
        }

        private void RunningStatusForm_Shown(object sender, EventArgs e)
        {

        }

        // 选择低速模式
        private void lowSpeedBtn_Click(object sender, EventArgs e)
        {
            if (this.lowSpeedBtn.ForeColor != Color.White)
            {
                this.lowSpeedBtn.BackColor = Color.FromArgb(253, 105, 89);
                this.lowSpeedBtn.ForeColor = Color.White;

                this.fullSpeedBtn.BackColor = Button.DefaultBackColor;
                this.fullSpeedBtn.ForeColor = Button.DefaultForeColor;
                
                this.lowSpeedBtn.Enabled = false;
                this.fullSpeedBtn.Enabled = false;
                
                this.processControl.ReduceProcessCpuUsage();
                               
                Thread.Sleep(1500);
                
                this.lowSpeedBtn.Enabled = true;
                this.fullSpeedBtn.Enabled = true;
            }
        }

        // 选择正常模式
        private void fullSpeedBtn_Click(object sender, EventArgs e)
        {
            if (this.fullSpeedBtn.ForeColor != Color.White)
            {               
                this.fullSpeedBtn.BackColor = Color.FromArgb(18, 173, 30);
                this.fullSpeedBtn.ForeColor = Color.White;

                this.lowSpeedBtn.BackColor = Button.DefaultBackColor;
                this.lowSpeedBtn.ForeColor = Button.DefaultForeColor;

                this.lowSpeedBtn.Enabled = false;
                this.fullSpeedBtn.Enabled = false;

                this.processControl.ResumeDefalutCpuAffinity();

                Thread.Sleep(1500);

                this.lowSpeedBtn.Enabled = true;
                this.fullSpeedBtn.Enabled = true;
            }
        }

        public void HideVideoEncoderSetting()
        {
            this.encoderPriority.Visible = false;
            this.priorityCB.Visible = false;
            this.fullSpeedBtn.Visible = false;
            this.lowSpeedBtn.Visible = false;
        }

        public void ShowVideoEncoderSetting()
        {
            this.encoderPriority.Visible = true;
            this.priorityCB.Visible = true;
            this.fullSpeedBtn.Visible = true;
            this.lowSpeedBtn.Visible = true;

            this.fullSpeedBtn.BackColor = Color.FromArgb(18, 173, 30);
            this.fullSpeedBtn.ForeColor = Color.White;

            this.lowSpeedBtn.BackColor = Button.DefaultBackColor;
            this.lowSpeedBtn.ForeColor = Button.DefaultForeColor;

            this.priorityCB.SelectedIndex = 1;
            this.priorityCB.SelectedIndex = 0;
        }

        private void priorityCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.processControl.setProcessName(this.encodingProgram);
            ComboBox comboBox = (ComboBox)sender;
            if (String.Equals(comboBox.Text, "低"))
            {
                this.processControl.SetProcessPriority(IDLE);
            } 
            else if (String.Equals(comboBox.Text, "低于正常"))
            {
                this.processControl.SetProcessPriority(BELOWNORMAL);
            }
            else if (String.Equals(comboBox.Text, "正常"))
            {
                this.processControl.SetProcessPriority(NORMAL);
            }
            else if (String.Equals(comboBox.Text, "高于正常"))
            {
                this.processControl.SetProcessPriority(ABOVENORMAL);
            }
            else if (String.Equals(comboBox.Text, "高"))
            {
                this.processControl.SetProcessPriority(HIGHPRIORITY);
            }
            else if (String.Equals(comboBox.Text, "实时"))
            {
                this.processControl.SetProcessPriority(REALTIME);
            }
        }

        private void fullSpeedBtn_VisibleChanged(object sender, EventArgs e)
        {
            if (((Button)sender).Visible == true && this.processControl.numberOfLogicalProcessors == -1)
            {
                this.fullSpeedBtn.Enabled = false;
                this.lowSpeedBtn.Enabled = false;
                InitializeNumberOfLogicalProcessors();            
            }
        }
   
        private void InitializeNumberOfLogicalProcessors()
        {
            this.processControl.numberOfLogicalProcessors = this.processControl.GetNumberOfLogicalProcessors();
            this.fullSpeedBtn.Enabled = true;
            this.lowSpeedBtn.Enabled = true;
        }
    }
}
