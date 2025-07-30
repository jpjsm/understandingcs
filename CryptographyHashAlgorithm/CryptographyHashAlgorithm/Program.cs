using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyHashAlgorithm
{
    class Program
    {
        public static byte[] foo(HashAlgorithm hash, byte[] buffer)
        {
            return hash.ComputeHash(buffer);
        }
        static void Main(string[] args)
        {
            MD5 md5 = MD5.Create();
            SHA1 sha1 = SHA1.Create();
            SHA256 sha256 = SHA256.Create();

            byte[] data = Encoding.UTF8.GetBytes("Hello World");
            byte[] hashMd5 = foo(md5, data);
            byte[] hashSha1 = foo(sha1, data);
            byte[] hashSha256 = foo(sha256, data);

        }
    }
}
