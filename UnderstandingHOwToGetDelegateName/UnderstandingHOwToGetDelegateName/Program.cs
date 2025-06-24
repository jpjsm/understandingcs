using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingHOwToGetDelegateName
{
    class Program
    {
        public delegate bool BoolOp(int a, int b);
        static void Main(string[] args)
        {
            TestDelegateReflection(1, 2, GT);
            TestDelegateReflection(1, 2, LT);

        }

        // The following code should help shed light on how to get delegate name
        /*
            public static string GetParamName(System.Reflection.MethodInfo method, int index)
            {
                string retVal = string.Empty;

                if (method != null && method.GetParameters().Length > index)
                    retVal = method.GetParameters()[index].Name;


                return retVal;
            }        
        */

        public static bool TestDelegateReflection(int a, int b, BoolOp op)
        {
            if (op == null)
            {
                throw new ArgumentNullException(nameof(op));
            }

            Type typeofProgram = typeof(Program);
            MethodInfo testDelegateReflectionInfo = typeofProgram.GetMethod("TestDelegateReflection");
            ParameterInfo opInfo = testDelegateReflectionInfo.GetParameters().Where(p => p.Name == "op").First();
            

            //ParameterInfo param =
            return op(a, b);
        }

        public static bool GT(int a, int b)
        {
            return a > b;
        }

        public static bool LT(int a, int b)
        {
            return a < b;
        }

        public static bool GE(int a, int b)
        {
            return a >= b;
        }

        public static bool LE(int a, int b)
        {
            return a <= b;
        }

        public static bool EQ(int a, int b)
        {
            return a == b;
        }

        public static bool NE(int a, int b)
        {
            return a != b;
        }
    }
}
