using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models.Filters
{
    public class SearchFilter
    {
        public string Property { get; set; }
        public object Value { get; set; }
        public Condition Condition { get; set; }
    }
}
