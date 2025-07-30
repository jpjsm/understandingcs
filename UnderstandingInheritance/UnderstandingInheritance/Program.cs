using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingInheritance
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class BaseClass
    {
        public string Id { get; set; }

        public static BaseClass LoadBaseClass(string content)
        {
            BaseClass results = new BaseClass();
            results.Id = content;
            return results;
        }
    }
}

namespace UnderstandingInheritance.Juanchines
{
    public class ChildClass : BaseClass
    {
        public string Name { get; set; }
    }

}

namespace UnderstandingInheritance.Pedrines
{
    public class ChildClass : BaseClass
    {
        public string Name { get; set; }
    }

}
