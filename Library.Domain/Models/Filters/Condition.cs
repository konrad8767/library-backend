using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models.Filters
{
    public enum Condition
    {
        Unknown,
        Not,
        Equal,
        Contains,
        GreaterThan,
        LesserThan,
        Multiselect
    }
}
