using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public static class LockExtensions
    {
        public static LockHelper Lock(this object obj, TimeSpan timeout)
        {
            bool lockToken = false;
            try
            {
                Monitor.TryEnter(obj, timeout, ref lockToken);
                if (lockToken)
                {
                    return new LockHelper(obj);
                }
                else
                    throw new TimeoutException("Failed to acquire stateGuard");
            }
            catch
            {
                if (lockToken)
                    Monitor.Exit(obj);
                throw;
            }
        }

        public struct LockHelper : IDisposable
        {
            private readonly object obj;

            public LockHelper(object obj)
            {
                this.obj = obj;
            }

            public void Dispose()
            {
                Monitor.Exit(obj);
            }
        }
    }
}
