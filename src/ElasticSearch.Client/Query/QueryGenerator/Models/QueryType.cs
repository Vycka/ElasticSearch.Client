using System;

namespace ElasticSearch.Client.Query.QueryGenerator.Models
{
    public enum QueryType
    {
        Must,
        MustNot,
        Should
    }

    public static class QueryTypeMapping
    {
        public static string GetName(QueryType queryType)
        {
            switch (queryType)
            {
                case QueryType.Must:
                    return "must";
                case QueryType.MustNot:
                    return "must_not";
                case QueryType.Should:
                    return "should";
                default:
                    throw new ArgumentOutOfRangeException("queryType");
            }
        }
    }
}