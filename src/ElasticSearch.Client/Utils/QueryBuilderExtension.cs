using System;
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
    }
}