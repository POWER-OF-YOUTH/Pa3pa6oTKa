using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class Extensions
    {
        public static byte[] GetSHA256(this string str) => new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(str));
    }
}
