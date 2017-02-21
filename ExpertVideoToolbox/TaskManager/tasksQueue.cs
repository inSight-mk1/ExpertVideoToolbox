using ExpertVideoToolbox.TaskManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertVideoToolbox.taskManager
{
    class tasksQueue
    {
        public int length;         // 现在的长度（不包含空）
        public videoTask[] tQueue;  // 队列
        public int max;            // 最大长度

        private int front;         // 队头指针
        private int rear;          // 队尾指针

        private Object lockObj = new Object();

        public tasksQueue(int taskLength, videoTask[] tq)
        {
            this.length = taskLength;
            this.max = taskLength;
            this.tQueue = new videoTask[taskLength + 1];  // 采用循环队列，浪费一个存储空间
            for (int i = 0; i < taskLength; i++)
            {
                this.tQueue[i] = tq[i];
            }
            this.tQueue[taskLength] = null;
            this.front = 0;
            this.rear = taskLength;  // 队尾指针指向队尾元素之后的一个空元素
        }

        public tasksQueue(int taskLength)  // 只传入值时，队列为空
        {
            this.length = 0;
            this.max = taskLength;
            this.tQueue = new videoTask[taskLength + 1];
            for (int i = 0; i < taskLength + 1; i++)
            {
                this.tQueue[i] = null;
            }
            this.front = this.rear = 0;
        }

        public videoTask pop()
        {
            lock(lockObj)  // 锁定，不允许其他线程同时运行这段代码
            {
                // 取出队首Task并且从队列中移除
                if (this.front == this.rear)
                {
                    return null;  // 队列为空返回null由运行池处理
                } else
                {
                    videoTask x = this.tQueue[this.front];
                    this.front = (this.front + 1) % (this.max + 1);

                    this.length--;  // 长度-1
                    return x;
                }
            }
        }

        public void add(videoTask t)
        {
            lock(lockObj)
            {
                // 在队列中添加
                // 此处不判断队列是否为满，请在上层对象中加以判断
                this.tQueue[this.rear] = t;
                this.rear = (this.rear + 1) % (this.max + 1);
                this.length++;  // 长度+1
            }
        }
       
        public void clear()
        {
            lock (lockObj)
            {
                // 清空队列
                for (int i = 0; i < this.max + 1; i++)
                {
                    this.tQueue[i] = null;
                }
                this.front = this.rear = 0;
                this.length = 0;
            }
        }
        
    }
}
