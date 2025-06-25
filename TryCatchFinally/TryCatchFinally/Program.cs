using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryCatchFinally
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextWriter inmemorystream = new StreamWriter(memoryStream);
            Console.WriteLine("Is Error Redirected: {0}", Console.IsErrorRedirected);

            try
            {
                try
                {
                    Console.SetError(inmemorystream);
                    Console.WriteLine("Inside try");
                    throw new Exception("Error!");
                    Console.WriteLine("Past throw1");
                }
                catch (Exception ex1)
                {
                    Console.WriteLine("Inside catch1");
                    throw;
                    Console.WriteLine("Past throw1.1");
                }
                finally
                {
                    Console.WriteLine("Inside finally1");
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine("Inside catch2");
                throw;
                Console.WriteLine("Past throw2");
            }
            finally
            {
                Console.WriteLine("Inside finally2");
            }

            Console.WriteLine("Past try-finally");
        }
    }
}
