using System;
using System.Runtime.Serialization;

namespace ElasticSearch.Query.QueryGenerator.Models
{
    public enum FilterType
    {
        Must,
        MustNot,
        Should
    }

    public static class FilterTypeMapping
    {
        public static string GetName(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Must:
                    return "must";
                case FilterType.MustNot:
                    return "must_not";
                case FilterType.Should:
                    return "should";
                default:
                    throw new ArgumentOutOfRangeException("filterType");
            }
        }
    }
}