﻿using System;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;

namespace ElasticSearch.Client.Query.QueryGenerator.Models
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