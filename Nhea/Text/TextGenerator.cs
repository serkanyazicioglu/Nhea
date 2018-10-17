using System;
using System.Text;

namespace Nhea.Text
{
    public static class TextGenerator
    {
        public static string Generate(int length)
        {
            return GetRandomText("QAZWSXEDCRFVTGBYHNUJMIKOLP1230456789", length);
        }

        public static string Generate(string characters, int length)
        {
            return GetRandomText(characters, length);
        }

        private static string GetRandomText(string characters, int length)
        {
            StringBuilder ret = new StringBuilder();
            Random rnd = new Random();
            
            for (int i = 0; i < length; i++)
            {
                ret.Append(characters[rnd.Next(characters.Length)]);
            }

            return ret.ToString();
        }
    }
}