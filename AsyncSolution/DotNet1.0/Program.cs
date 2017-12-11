using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet1._0
{
    class Program
    {
        /*
         *   异步编程方法：
         * 1        ThreadPool.QueueUserWorkItem
         * 2        Timers   间隔时间到都，在线程池中获取线程值执行任务
         * 3        The Asynchronous Programming Model(APM)   即 BeginXXX，EndXXX
         * 
         * 
         *          2.0
         * 1        使用线程池方法可带一个参数
         * 2        使用SynchronizationContext.Post   方法执行异步回调  该方法等效于  Dispatcher.Invoke 和 Control.Invoke
         * 3        EAP  事件模式
         */
        static void Main(string[] args)
        {
            Console.WriteLine($"MainThread Id: {Thread.CurrentThread.ManagedThreadId}");
            new Thread(new ThreadStart(MonitorNetwork)).Start();

            Thread thread = new Thread(new ThreadStart(AbortNetWork));
            thread.Start();
            Thread.Sleep(1000);
            thread.Abort(); //在调用线程引发ThreadAbortException异常来终止线程
            //thread.Join();//阻塞主线程的，等待thread线程结束
            Console.WriteLine("MainThread End");
            Console.ReadKey();
        }

        static void MonitorNetwork()
        {
            Console.WriteLine($"CurrentThread Id: {Thread.CurrentThread.ManagedThreadId}");
        }

        static void AbortNetWork()
        {
            try
            {
                Console.WriteLine($"CurrentThread Id: {Thread.CurrentThread.ManagedThreadId}");
                for (int i = 0; i < 1000000000; i++)
                {
                    for (int j = 0; j < 1000000000; j++)
                        j %= 99999;
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
