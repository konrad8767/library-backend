using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class ListResult<T>
    {
        public ListResult(IEnumerable<T> value, int count)
        {
            Value = value;
            Count = count;
        }

        public IEnumerable<T> Value { get; }
        public int Count { get; }
    }
}
