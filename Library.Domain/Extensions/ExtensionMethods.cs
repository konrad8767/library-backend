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
    }
}
