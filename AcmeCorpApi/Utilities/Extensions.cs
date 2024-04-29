using System;
using System.Text;

namespace AcmeCorpApi.Utils.Extensions
{
    public static class ExtensionMethods
    {
        public static string GetString(this byte[] data)
        {
            try { return Encoding.ASCII.GetString(data); }
            catch { return null; }
        }
        
        public static byte[] ToBytes(this string input)
        {
            try { return Encoding.ASCII.GetBytes(input); }
            catch { return null; }
        }

        public static byte[] ToBytes(this int value)
        {
            try { return BitConverter.GetBytes(value); }
            catch { return null; }
        }
    }
}