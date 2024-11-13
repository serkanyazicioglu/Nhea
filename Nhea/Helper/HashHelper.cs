using System.Security.Cryptography;
using System.Text;

namespace Nhea.Helper
{
    public static class HashHelper
    {
        public static string SHA1Hash(string data)
        {
            string hash = string.Empty;
            byte[] crypto = SHA1.HashData(Encoding.ASCII.GetBytes(data));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        public static string SHA256Hash(string data)
        {
            string hash = string.Empty;
            byte[] crypto = SHA256.HashData(Encoding.ASCII.GetBytes(data));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }
    }
}
