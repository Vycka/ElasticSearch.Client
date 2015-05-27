using System;
using System.Collections.Generic;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.Query.IndexListGenerator;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Serializer;

namespace ElasticSearch.Client.Utils
{
    public static class QueryBuilderExtension
    {
        public static void PrintQuery(this QueryBuilder queryBuilder)
        {
            JsonQuerySerializer querySerializer = new JsonQuerySerializer { PrettyPrint = true };

            Console.Out.WriteLine(querySerializer.BuildJsonQuery(queryBuilder));
        }

        public static void PrintQuery(this QueryBuilder queryBuilder, List<ElasticSearchIndexDescriptor> indexes)
        {
            SmartIndexListBuilder indexListBuilder = new SmartIndexListBuilder(indexes, queryBuilder);
            Console.Out.WriteLine("INDEXES: " + String.Join(", ", indexListBuilder.BuildLookupIndexes()));

            PrintQuery(queryBuilder);
        }
    }
}