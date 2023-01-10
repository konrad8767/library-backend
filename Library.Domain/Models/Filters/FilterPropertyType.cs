using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models.Filters
{
    public static class FilterPropertyType
    {
        public static string TEXT => "text";
        public static string DATE => "date";
        public static string LIST => "list";
        public static string BOOLEAN => "boolean";
        public static string NUMBER => "number";
    }
}
