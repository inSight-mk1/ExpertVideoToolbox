using ExpertVideoToolbox.TaskManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertVideoToolbox.cmdCodeGenerator
{
    class filePathStorage
    {
        private videoTask[] storage;
        private int count;
        private int maxCount;
        
        public filePathStorage(int maxCount)
        {
            this.maxCount = maxCount;
            this.count = 0;
            this.storage = new videoTask[maxCount];  // 根据文件个数分配存储空间
        }

        public videoTask get(int index)
        {
            return storage[index];
        }

        public void add(videoTask t)
        { 
            try
            {
                if (this.count == maxCount)
                {
                    throw new Exception("Overflow");
                } else
                {
                    this.storage[count++] = t;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not add any more tasks. Original error: " + ex.Message);
            }
        }

        public int Count()
        {
            return this.maxCount;
        }
    }
}
