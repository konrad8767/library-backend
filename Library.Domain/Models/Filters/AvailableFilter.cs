using System.Collections.Generic;

namespace Library.Domain.Models.Filters
{
    public class AvailableFilter
    {
        public AvailableFilter() { }

        public AvailableFilter(string property, FilterType type)
        {
            property = property;
            PropertyType = type switch
            {
                FilterType.LIST => FilterPropertyType.LIST,
                FilterType.BOOLEAN => FilterPropertyType.BOOLEAN,
                FilterType.TEXT => FilterPropertyType.TEXT,
                FilterType.DATE => FilterPropertyType.DATE,
                FilterType.NUMBER => FilterPropertyType.NUMBER,
                _ => throw new System.NotImplementedException()
            };

            Conditions = type switch
            {
                FilterType.LIST => new[] { Condition.Multiselect },
                FilterType.BOOLEAN => new[] { Condition.Equal },
                FilterType.TEXT => new[] { Condition.Equal, Condition.Contains },
                FilterType.DATE => new[] { Condition.Equal, Condition.GreaterThan, Condition.LesserThan },
                FilterType.NUMBER => new[] { Condition.Equal, Condition.GreaterThan, Condition.LesserThan },
                _ => throw new System.NotImplementedException()
            };
        }
        public string Property { get; set; }
        public string PropertyType { get; set; }
        public IList<Condition> Conditions { get; set; }
        public IList<object> Values { get; set; }
    }
}
