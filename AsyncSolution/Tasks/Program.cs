using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            TaskScheduler.UnobservedTaskException += HandleTaskExceeptions;
            HandleException();
            CancelTask(cts.Token);
            TestSmallBusiness();
            LazyTest();
            Thread.Sleep(200);
            cts.Cancel();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Clock clock = new Clock();
            ParallelTest.Test();
            Console.ReadKey();
        }



        static void CancelTask(CancellationToken token)
        {
            Task.Run(() =>
            {
                try
                {
                    int i = 0;
                    while (true)
                    {
                        i++;
                        token.ThrowIfCancellationRequested();
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        static void HandleException()
        {
            Task t = Task.Factory.StartNew(() => ThrowException());
            try
            {
                t.Wait(); //如果调用此方法，任务内的异常将在这里抛出，上面的UnobservedTaskException无效
            }
            catch (AggregateException errors)
            {

                foreach (Exception error in errors.Flatten().InnerExceptions)
                {
                    Console.WriteLine($"{error.GetType()}: {error.Message}");
                }
            }
        }

        static void HandleTaskExceeptions(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine(sender is Task);
            foreach (Exception error in e.Exception.InnerExceptions)
            {
                Console.WriteLine(error.Message);
            }
            e.SetObserved();
        }

        private static void ThrowException()
        {
            //Thread.Sleep(1000);
            throw new Exception("抛出一个异常");
        }

        static Task<string> BetterDownloadWebPageAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);
            IAsyncResult ar = request.BeginGetResponse(null, null);

            Task<string> downloadTask = Task.Factory.FromAsync(ar, iar =>
            {
                using (WebResponse response = request.EndGetResponse(iar))
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });

            return downloadTask;
        }

        static void TestSmallBusiness()
        {
            SmallBusiness bus = new SmallBusiness(0, 100);
            Console.WriteLine($"Amount: {bus.cash}  Receivables {bus.receivables}");
            Parallel.For(0, 100, o => bus.ReceivePayment(1));

            Console.WriteLine($"Amount: {bus.cash}  Receivables {bus.receivables}");
        }

        static void LazyTest()
        {
            Lazy<Person> lazyPerson = new Lazy<Person>();
            Console.WriteLine("Lazy obj created");
            Console.WriteLine($"has person been created {lazyPerson.IsValueCreated}");
            Console.WriteLine("Setting Name");
            lazyPerson.Value.Name = "Andy";
            Console.WriteLine("Setting Age");
            lazyPerson.Value.Age = 21;

            Person andy = lazyPerson.Value;
            Console.WriteLine(andy);
        }
    }
}
