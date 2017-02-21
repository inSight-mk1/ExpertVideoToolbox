using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertVideoToolbox.TaskManager
{
    class videoTask
    {
        private string filePath;
        private string setting;

        public videoTask(string path)  
        {
            this.filePath = path;
        }

        public videoTask(string path, string setting)
        {
            this.filePath = path;
            this.setting = setting;
        }

        public string printTask()
        {
            return this.filePath;
        }

        public string getFP()
        {
            return this.filePath;
        }

        public string getSetting()
        {
            return this.setting;
        }
    }
}
