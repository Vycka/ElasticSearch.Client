﻿using System;

namespace ElasticSearch.Client.ElasticSearch
{
    public class ElasticSearchQuery
    {
        public string QueryJson { get; private set; }
        public string[] LookupIndexes { get; private set; }

        public ElasticSearchQuery(string queryJson, string[] lookupIndexes)
        {
            if (queryJson == null) 
                throw new ArgumentNullException("queryJson");
            if (lookupIndexes == null) 
                throw new ArgumentNullException("lookupIndexes");

            QueryJson = queryJson;
            LookupIndexes = lookupIndexes;
        }
    }
}
