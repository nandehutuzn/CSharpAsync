using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class Cache
    {
        private readonly List<string> items = new List<string>();
        ReaderWriterLockSlim guard = new ReaderWriterLockSlim();

        public IEnumerable<string> GetItem(string tag)
        {
            guard.EnterReadLock();
            try
            {
                return items.Where(o => o.Contains(tag)).ToList();
            }
            finally
            {
                guard.ExitReadLock();
            }
        }

        public void AddItem(string item)
        {
            guard.EnterWriteLock();
            try
            {
                items.Add(item);
            }
            finally
            {
                guard.ExitWriteLock();
            }
        }
    }
}
