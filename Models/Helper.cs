using System;
using System.Text;
using System.Security.Cryptography;

namespace Notes.Models
{
    public static class Helper
    {
        /// <summary>
        /// Create MD5 hash from UTF-8 string
        /// </summary>
        /// <param name="str">UTF-8 string</param>
        /// <returns>String value of MD5 hash</returns>
        public static string Hash(string str)
        {
            using (var md5 = MD5.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(str);
                var hashBytes = md5.ComputeHash(passwordBytes);
                var hash = BitConverter.ToString(hashBytes);
                return hash;
            }
        }
    }
}
