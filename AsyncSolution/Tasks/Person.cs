using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class Person
    {
        public Person()
        {
            //Thread.Sleep(2000);
            Console.WriteLine("Created");
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }
    }
}
