using System;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Serializer;
using Newtonsoft.Json;

namespace ElasticSearch.Playground.Utils
{
    public static class QueryBuilderExtension
    {
        

        public static void PrintQuery(this QueryBuilder queryBuilder)
        {
            JsonQuerySerializer querySerializer = new JsonQuerySerializer();
            querySerializer.PrettyPrint = true;

            Console.Out.WriteLine(querySerializer.BuildJsonQuery(queryBuilder));
        }
    }
}