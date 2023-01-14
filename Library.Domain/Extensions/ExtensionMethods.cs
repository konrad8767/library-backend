using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Extensions
{
    public static class ExtensionMethods
    {
        public static string ToStrUp(this object value) => value.ToString().ToUpper();

        public static DateTime ToDateTime(this object value)
        {
            DateTime.TryParse(value.ToString(), out var date);
            return date;
        }

        public static DateTime ToDate(this object value)
        {
            DateTime.TryParse(value.ToString(), out var date);
            return date.Date;
        }

        public static DateTime EndOfDay(this DateTime value)
        {
            return value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
