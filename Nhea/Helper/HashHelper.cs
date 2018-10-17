using System;
using System.Security.Cryptography;
using System.Text;

namespace Nhea.Helper
{
    public static class HashHelper
    {
        public static string SHA1Hash(string data)
        {
            SHA1Managed crypt = new SHA1Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetByteCount(data));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        public static string SHA256Hash(string data)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetByteCount(data));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }
    }
}
