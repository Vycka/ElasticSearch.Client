using System;
using ElasticSearch.Playground.Query.QueryGenerator.QueryComponents;

namespace ElasticSearch.Playground.Query.QueryGenerator.Models
{
    public class QueryItem
    {
        public readonly QueryType QueryType;
        public readonly IQueryComponent QueryComponent;

        public QueryItem(QueryType queryType, IQueryComponent queryComponent)
        {
            if (queryComponent == null) 
                throw new ArgumentNullException("queryComponent");

            QueryComponent = queryComponent;
            QueryType = queryType;
        }
    }
}