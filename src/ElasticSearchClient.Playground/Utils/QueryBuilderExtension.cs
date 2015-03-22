using System.Diagnostics;
using ElasticSearchClient.Query.QueryGenerator;
using Newtonsoft.Json;

namespace ElasticSearchClient.Playground.Utils
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

            Debug.WriteLine(result);
        }
    }
}