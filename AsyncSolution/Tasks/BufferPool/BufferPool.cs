using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks.BufferPool
{
    public class BufferPool
    {
        private SemaphoreSlim guard;
        private List<LOHBuffer> buffers;

        public BufferPool(int maxSize)
        {
            guard = new SemaphoreSlim(maxSize);
            buffers = new List<LOHBuffer>(maxSize);
        }

        public IBufferRegistration GetBuffer()
        {
            guard.Wait();
            lock (buffers)
            {
                IBufferRegistration freeBuffer = null;

                foreach (LOHBuffer buffer in buffers)
                {
                    if (!buffer.InUse)
                    {
                        buffer.InUse = true;
                        freeBuffer = new BufferReservation(this, buffer);
                    }
                }

                if (freeBuffer == null)
                {
                    var buffer = new LOHBuffer();
                    buffer.InUse = true;
                    buffers.Add(buffer);
                    freeBuffer = new BufferReservation(this, buffer);
                }

                return freeBuffer;
            }
        }
        
        internal void Release(LOHBuffer buffer)
        {
            buffer.InUse = false;
            guard.Release();
        }
    }
}
