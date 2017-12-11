using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Monitor实现的信号量
    /// </summary>
    public class MonitorSemaphore  
    {
        private int currentCount;
        private readonly int maxCount;
        private object guard = new object();

        public MonitorSemaphore(int initialCount, int maxCount)
        {
            this.currentCount = initialCount;
            this.maxCount = maxCount;
        }

        public void Enter()
        {
            lock (guard)
            {
                while (currentCount == maxCount)
                {
                    Monitor.Wait(guard);
                }
                currentCount++;
            }
        }

        public void Exit()
        {
            lock (guard)
            {
                currentCount--;
                Monitor.Pulse(guard);
            }
        }

        public int CurrentCount { get { return currentCount; } }
    }
}
