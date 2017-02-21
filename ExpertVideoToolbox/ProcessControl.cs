using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertVideoToolbox
{
    public class ProcessControl
    {
        const int IDLE = 64;
        const int BELOWNORMAL = 16384;
        const int NORMAL = 32;
        const int ABOVENORMAL = 32768;
        const int HIGHPRIORITY = 128;
        const int REALTIME = 256;
        
        private string processName;

        public int numberOfLogicalProcessors = -1;

        private Process cmdProcess;

        public void setProcessName(string name)
        {
            this.processName = name;
        }
        
        public ProcessControl()
        {                      
            cmdProcess = new System.Diagnostics.Process();

            cmdProcess.StartInfo.FileName = "cmd";

            // 必须禁用操作系统外壳程序  
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.Start();
        }

        // 耗时操作
        public int GetNumberOfLogicalProcessors()
        {            
            return Environment.ProcessorCount;
        }

        // 设置进程的优先级
        public void SetProcessPriority(int priority)
        {
            string command = "wmic process where name=" + "\"" + processName + "\"" + " CALL setpriority ";
            switch(priority)
            {
                case IDLE:
                    command += IDLE.ToString();
                    break;
                case BELOWNORMAL:
                    command += BELOWNORMAL.ToString();
                    break;
                case NORMAL:
                    command += NORMAL.ToString();
                    break;
                case ABOVENORMAL:
                    command += ABOVENORMAL.ToString();
                    break;
                case HIGHPRIORITY:
                    command += HIGHPRIORITY.ToString();
                    break;
                case REALTIME:
                    command += REALTIME.ToString();
                    break;                    
            }

            MaintainCmdProcessRunning();
            this.cmdProcess.StandardInput.WriteLine(command);
        }

        // 设置进程的cpu相关性，限制视频编码器占用逻辑处理器数为满载的1/4（默认值，且至少有一个逻辑处理器）
        public void ReduceProcessCpuUsage(int slice = 4)
        {
            string command = "PowerShell \"$Process = Get-Process " + changeFileExtName(this.processName) + "; $Process.ProcessorAffinity=";

            int affinity = 0;
            int k = this.numberOfLogicalProcessors;
            
            if (slice <= 1)
            {
                affinity = (int)Math.Pow(2, k) - 1;
            }
            else
            {
                int numberOfLimitedLogicalProcessors = k / slice;
                if (numberOfLimitedLogicalProcessors < 1)
                {
                    numberOfLimitedLogicalProcessors = 1;
                }

                // 设置affinity的值，如有8个处理器，affinity则是8位二进制数，每一位上0、1表示该处理器的开关状态，越高位表示的CPU编号越大。
                // 如：测试机i7 四核八线程视为八个处理器，10100000表示打开了CPU7和CPU5
                // 在PowerShell命令中affinity值要转换为十进制数
                k--;
                for (int i = 0; i < numberOfLimitedLogicalProcessors; i++)
                {
                    affinity += (int)Math.Pow(2, k);
                    k -= 2;
                }
            }
            
            command += affinity.ToString() + "\"";

            MaintainCmdProcessRunning();
            this.cmdProcess.StandardInput.WriteLine(command);
        }

        public void ResumeDefalutCpuAffinity()
        {
            ReduceProcessCpuUsage(1);  // 设置进程使用的逻辑处理器数量为总数除以1，即恢复到满载状态
        }

        // 检查进程是否在运行，如果不是则重新开始
        private void MaintainCmdProcessRunning()
        {
            if (this.cmdProcess.HasExited)  // 如果进程已经退出则再次开始
            {
                this.cmdProcess.Start();
            }
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

        /// <summary>
        /// A utility class to determine a process parent.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ParentProcessUtilities
        {
            // These members must match PROCESS_BASIC_INFORMATION
            internal IntPtr Reserved1;
            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;

            [DllImport("ntdll.dll")]
            private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

            /// <summary>
            /// Gets the parent process of the current process.
            /// </summary>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess()
            {
                return GetParentProcess(Process.GetCurrentProcess().Handle);
            }

            /// <summary>
            /// Gets the parent process of specified process.
            /// </summary>
            /// <param name="id">The process id.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(int id)
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }

            /// <summary>
            /// Gets the parent process of a specified process.
            /// </summary>
            /// <param name="handle">The process handle.</param>
            /// <returns>An instance of the Process class or null if an error occurred.</returns>
            public static Process GetParentProcess(IntPtr handle)
            {
                ParentProcessUtilities pbi = new ParentProcessUtilities();
                int returnLength;
                int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
                if (status != 0)
                    return null;

                try
                {
                    return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
                }
                catch (ArgumentException)
                {
                    // not found
                    return null;
                }
            }
        }
    }
}
