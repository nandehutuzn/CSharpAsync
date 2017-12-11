using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class SmallBusiness
    {
        public int cash;
        public int receivables;
        private readonly object stateGuard = new object();

        public SmallBusiness(int cash, int receivables)
        {
            this.cash = cash;
            this.receivables = receivables;
        }

        public void ReceivePayment(int amount)
        {
            //bool lockTaken = false;
            //try
            //{
            //    Monitor.Enter(stateGuard, ref lockTaken);
            //    cash += amount;
            //    receivables -= amount;
            //}
            //finally
            //{
            //    if (lockTaken)
            //        Monitor.Exit(stateGuard);
            //}

            ////等效于
            //lock (stateGuard)//lock(this)
            //{
            //    cash += amount;
            //    receivables -= amount;
            //}

            //等效于
            using (stateGuard.Lock(TimeSpan.FromMilliseconds(30)))
            {
                cash += amount;
                receivables -= amount;
            }
        }
    }
}
