using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertVideoToolbox.cmdCodeGenerator
{
    class taskSetting
    {
        public string name;
        
        public string encoder;         // 视频编码器
        public string outputFormat;    // 输出格式
        public string encoderSetting;  // 参数

        public string audioEncoder;
        public string audioProfile;
        public string audioCodecMode;
        public string audioQuality;
        public string audioKbps;

        public taskSetting(string name,  // 名称
                           string e,     // 视频编码器
                           string of,    // 输出格式
                           string es,    // 编码器参数
                           string aE,    // 音频编码器
                           string aP,    // 音频Profile
                           string aMode, // 音频编码模式
                           string aQ,    // 音频质量或码率
                           string aK)
        {
            this.encoder = e;
            this.outputFormat = of;
            this.encoderSetting = es;
            this.audioEncoder = aE;
            this.audioProfile = aP;
            this.audioCodecMode = aMode;         
            this.audioQuality = aQ;
            this.audioKbps = aK;
            this.name = name;
        }
    }
}
