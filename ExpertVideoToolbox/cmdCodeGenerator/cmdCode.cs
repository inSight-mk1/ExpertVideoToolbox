using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertVideoToolbox.cmdCodeGenerator
{
    class cmdCode
    {
        const int AUDIOENCODE = 0;
        const int VIDEOENCODE = 1;
        const int MUXER = 2;
        const int AUDIOCOPY = 3;
        const int DELETEVIDEOTEMP = 4;
        const int DELETEAUDIOTEMP = 5;

        const int ONLYVIDEO = 0;
        const int SUPPRESSAUDIO = 1;
        const int COPYAUDIO = 2;
        
        private string filePath;
        private taskSetting ts;
        private string outputFolderPath;
        private string outputAudioFormat;

        public cmdCode(string fp, taskSetting t, string ofp)
        {
            this.filePath = fp;
            this.ts = t;
            this.outputFolderPath = ofp;
        }

        public void setFilePath(string fp)
        {
            this.filePath = fp;
        }

        public int taskType()
        {           
            if (String.Equals(this.ts.audioEncoder, "无音频流"))
            {
                return ONLYVIDEO;
            } else if (String.Equals(this.ts.audioEncoder, "复制音频流"))
            {
                MediaInfo MI = new MediaInfo();
                string duration;
                MI.Open(this.filePath);
                duration = MI.Get(StreamKind.Audio, 0, 69);
                this.outputAudioFormat = MI.Get(StreamKind.Audio, 0, "Format");
                if (String.IsNullOrWhiteSpace(duration))
                {
                    return ONLYVIDEO;
                } else
                {
                    if (CheckBoxCompatibility(this.outputAudioFormat, this.ts.outputFormat))
                    {
                        return COPYAUDIO;
                    }
                    else
                    {
                        this.outputAudioFormat = "aac";
                        return SUPPRESSAUDIO;
                    }                    
                }                
            } else
            {
                MediaInfo MI = new MediaInfo();
                string duration;
                MI.Open(this.filePath);
                duration = MI.Get(StreamKind.Audio, 0, 69);
                if (String.IsNullOrWhiteSpace(duration))
                {
                    return ONLYVIDEO;
                }
                else
                {
                    this.outputAudioFormat = "aac";
                    return SUPPRESSAUDIO;
                } 
            }
        }

        public string cmdCodeGenerate(int mode)
        {
            string fp = this.filePath;

            string qaacPath = System.Windows.Forms.Application.StartupPath + "\\tools\\qaac\\qaac.exe";
            string ffmpegPath = System.Windows.Forms.Application.StartupPath + "\\tools\\ffmpeg\\ffmpeg.exe";
            string x264Path = System.Windows.Forms.Application.StartupPath + "\\tools\\x26x\\" + ts.encoder;
            string mp4BoxPath = System.Windows.Forms.Application.StartupPath + "\\tools\\mp4Box\\MP4Box.exe";
            //string lsmashRemuxerPath = System.Windows.Forms.Application.StartupPath + "\\tools\\L-Smash\\remuxer.exe";
            string mkvmergePath = System.Windows.Forms.Application.StartupPath + "\\tools\\mkvmerge\\mkvmerge.exe";
        
            string code = "";

            string suffix = this.taskType() == ONLYVIDEO ? "_evt" : "_temp";
            if (String.IsNullOrWhiteSpace(this.outputFolderPath))
            {
                this.outputFolderPath = getPureFolderName(fp);
            }
 
            string outputFilePath = this.outputFolderPath + getPureFileName(fp) + "_" + this.ts.name + "." + ts.outputFormat;
            string videoTempPath = this.outputFolderPath + ts.name + "_temp" + ".mp4";
            string mp4BoxTempPath = this.outputFolderPath.Substring(0, this.outputFolderPath.Length - 1);  // tmp路径末尾不能有斜杠，否则parse出错（坑爹的gpac）
            string audioTempPath = this.outputFolderPath + getPureFileName(fp) + "_audioTemp." + this.outputAudioFormat;

            switch (mode)
            {               
                case VIDEOENCODE:      
                    code = "\"" + x264Path + "\"" + " " + ts.encoderSetting + " -o " + "\"" + videoTempPath + "\"" + " " + "\"" + fp + "\"";
                    break;
                case AUDIOCOPY:
                    code = "\"" + ffmpegPath + "\"" + " -i " + "\"" + fp + "\"" + " -vn -sn -async 1 -c:a copy -y -map 0:a:0 " + "\"" + audioTempPath + "\"";                    
                    break;
                case MUXER:
                    if (String.Equals(ts.outputFormat, "mp4", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (this.taskType() == ONLYVIDEO)
                        {
                            code = "\"" + mp4BoxPath + "\"" + " -add " + "\"" + videoTempPath + "\"" +
                                      " " + "\"" + outputFilePath + "\"" + " -tmp " + "\"" + mp4BoxTempPath + "\"";
                        }
                        else
                        {
                            code = "\"" + mp4BoxPath + "\"" + " -add " + "\"" + videoTempPath + "\"" +
                                " -add " + "\"" + audioTempPath + "\"" + " " + "\"" + outputFilePath + "\"" + " -tmp " + "\"" + mp4BoxTempPath + "\"";
                        }
                        
                        // tmp文件夹设定在输出位置，不设置的话默认C盘，当C盘空间不足以存放一个视频压制后的.mp4文件时任务就会失败
                        // 上述的问题解决了，暂时不考虑其他的mp4混流器了，mp4Box自用近两年从未crash，稳定性还是有保障的
                    }
                    else if (String.Equals(ts.outputFormat, "mkv", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (this.taskType() == ONLYVIDEO)
                        {
                            code += "\"" + mkvmergePath + "\"" + " -o " + "\"" + outputFilePath + "\"" + " " + "\"" + videoTempPath +
                                    "\"";
                        }
                        else
                        {
                            code += "\"" + mkvmergePath + "\"" + " -o " + "\"" + outputFilePath + "\"" + " " + "\"" + videoTempPath +
                                    "\"" + " " + "\"" + audioTempPath + "\"";
                        }
                    }
                    
                    break;
                
                case DELETEVIDEOTEMP:
                    code = videoTempPath;
                    break;
                case DELETEAUDIOTEMP:
                    code = audioTempPath;
                    break;

                case AUDIOENCODE:
                    if (String.Equals(this.ts.audioEncoder, "qaac", StringComparison.CurrentCultureIgnoreCase)
                        || String.Equals(this.ts.audioEncoder, "复制音频流", StringComparison.CurrentCultureIgnoreCase))
                    {
                        code = "\"" + ffmpegPath + "\"" + " -i " + "\"" + fp + "\"" + " -vn -sn -v 0 -async 1 -c:a pcm_s16le -f wav pipe:|" + "\"" + qaacPath + "\"" + " - --ignorelength";
                        if (string.Equals(this.ts.audioProfile, "HE-AAC"))
                        {
                            code += " --he";
                        }

                        if (string.Equals(this.ts.audioProfile, "ALAC"))
                        {
                            code += " -A";
                            break;
                        }

                        if (string.Equals(this.ts.audioCodecMode, "TVBR"))
                        {
                            code += " -V " + this.ts.audioQuality.ToString();
                        }
                        else if (string.Equals(this.ts.audioCodecMode, "CVBR"))
                        {
                            code += " -v " + this.ts.audioKbps.ToString();
                        }
                        else if (string.Equals(this.ts.audioCodecMode, "ABR"))
                        {
                            code += " -a " + this.ts.audioKbps.ToString();
                        }
                        else if (string.Equals(this.ts.audioCodecMode, "CBR"))
                        {
                            code += " -c " + this.ts.audioKbps.ToString();
                        }

                        code += " -o " + "\"" + audioTempPath + "\"";
                    }
                    break;
                default:
                    break;
            }   

            //code = "\"" + ffmpegPath + "\"" + " -i " + "\"" + fp + "\"" + " -vn -sn -v 0 -async 1 -c:a pcm_s16le -f wav pipe:|" + "\"" + qaacPath + "\"" + " - --ignorelength";
            
            //MessageBox.Show(code);
            return code;             
        }

        private bool CheckBoxCompatibility(string audioFormat, string fileFormat)
        {
            if (String.Equals(fileFormat, "mkv", StringComparison.CurrentCultureIgnoreCase))
            {
                if (String.Equals(audioFormat, "AAC", StringComparison.CurrentCultureIgnoreCase)
                 || String.Equals(audioFormat, "FLAC", StringComparison.CurrentCultureIgnoreCase)
                 || String.Equals(audioFormat, "DTS", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (String.Equals(fileFormat, "mp4", StringComparison.CurrentCultureIgnoreCase))
            {
                if (String.Equals(audioFormat, "AAC", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        private bool losslessCheckByExtension(string extension)
        {
            string[] losslessExt = { "wav", "flac", "ape" };
            return this.multiStringEqualCheck(losslessExt, extension);
        }

        private bool multiStringEqualCheck(string[] targets, string source)
        {
            int len = targets.Length;
            for (int i = 0; i < len; i++)
            {
                if (string.Equals(targets[i], source))
                {
                    return true;
                }        
            }
            return false;
        }

        private string changeFileExtName(string originFp, string ext = "")
        {
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

        // 获得纯净的文件名，不含路径和扩展名和点
        private string getPureFileName(string fp)
        {
            string[] s = fp.Split(new char[] { '\\' });
            int length = s.Length;
            string fileNameWithExt = s[length - 1];
            return changeFileExtName(fileNameWithExt, "");
        }

        private string getPureFolderName(string fp)
        {
            string[] s = fp.Split(new char[] { '\\' });
            int length = s.Length;
            string dest = "";
            for (int i = 0; i < length - 1; i++)
            {
                dest += s[i];
                dest += "\\";
            }
            return dest;
        }
    }
}
