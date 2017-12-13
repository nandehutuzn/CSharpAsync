using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    class ParallelTest
    {
        public static void Test()
        {
            Console.WriteLine("Parallel.Break 方法");
            Parallel.For(1, 100, (i, loopState) =>
             {
                 if (i >= 50)
                 {
                     Console.WriteLine($"{Task.CurrentId} : Breaking on {i}");
                     loopState.Break();  // i < 50 的基本都会执行完，i>=50不再执行
                     return;
                 }
                 Console.WriteLine($"{Task.CurrentId} {i} ");
             });

            Console.WriteLine("Parallel.Stop 方法");
            Parallel.For(1, 100, (i, loopState) =>
            {
                if (i >= 50)
                {
                    Console.WriteLine($"{Task.CurrentId} : Stop on {i}");
                    loopState.Stop();  // (立刻)退出循环，不管后面循环条件是否满足  i < 50
                    return;
                }
                Console.WriteLine($"{Task.CurrentId} {i} ");
            });
        }
    }
}
