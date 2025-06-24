using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingClassName
{
    public class Foo
    {
        public string ClassName { get; } 

        public Foo()
        {
            ClassName = this.GetType().Name;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Foo foo = new Foo();

            Console.Write("Class name: {0}", foo.ClassName);
        }
    }
}
