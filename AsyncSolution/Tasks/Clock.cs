using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class Clock  //只在UI线程有用
    {
        public int Hour { get { return DateTime.Now.Hour; } }

        public int Minutes { get { return DateTime.Now.Minute; } }

        public int Seconds { get { return DateTime.Now.Second; } }

        public event EventHandler<EventArgs> Tick = delegate { };

        private Timer timer;
        private SynchronizationContext syncCtx;

        public Clock()
        {
            Console.WriteLine($"Constructs Thread Id {Thread.CurrentThread.ManagedThreadId}");
            syncCtx = SynchronizationContext.Current ?? new SynchronizationContext();
            //timer = new Timer(OnTick, null, 1000, 1000);
        }

        private void OnTick(object state)
        {
            syncCtx.Post(o => Console.WriteLine($"CurrentThread Id {Thread.CurrentThread.ManagedThreadId}"), null);
        }
    }
}
