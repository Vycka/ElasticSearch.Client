using System;
using System.Diagnostics;
using ElasticSearch.Client.Query.QueryGenerator;
using Newtonsoft.Json;

namespace ElasticSearch.Playground.Utils
{
    public static class QueryBuilderExtension
    {
        public static void PrintQuery(this QueryBuilder queryBuilder)
        {
            string result = JsonConvert.SerializeObject(
                queryBuilder.BuildRequestObject(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                }
            );

            Console.WriteLine(result);
        }
    }
}