using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenerateStrongPassword
{
    public static class GenerateStrongPassword
    {
        public static string GeneratePassword(int passwordLength = 15)
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] randombyte = new byte[1];
            char[] password = (new string(' ', passwordLength)).ToCharArray();

            for (int i = 0; i < passwordLength; i++)
            {
                do
                {
                    rngCsp.GetNonZeroBytes(randombyte);
                    randombyte[0] &= 0x7F;
                    password[i] = Convert.ToChar(randombyte[0]);
                } while (randombyte[0] < 33 || randombyte[0] > 126);
            }

            return new string(password);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(GenerateStrongPassword.GeneratePassword());
            }
        }


    }
}
