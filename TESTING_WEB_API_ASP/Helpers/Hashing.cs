using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TESTING_WEB_API_ASP.Helpers
{
    public static class Hashing
    {
        public static string GetMD5FileHash(byte[] fileBytes)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(fileBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
