using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.BufferPool
{
    public class BufferReservation : IBufferRegistration
    {
        private readonly BufferPool pool;
        private readonly LOHBuffer buffer;

        public BufferReservation(BufferPool pool, LOHBuffer buffer)
        {
            this.pool = pool;
            this.buffer = buffer;
        }

        public byte[] Buffer
        {
            get
            {
                return buffer.Buffer;
            }
        }

        public void Dispose()
        {
            pool.Release(buffer);
        }
    }
}
