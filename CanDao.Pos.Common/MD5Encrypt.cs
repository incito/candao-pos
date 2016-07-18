using System;
using System.Security.Cryptography;
using System.Text;

namespace CanDao.Pos.Common
{
    public class MD5Encrypt
    {
        public static string Encrypt(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(data));
            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}