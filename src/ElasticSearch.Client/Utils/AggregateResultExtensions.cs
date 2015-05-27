using System;
using ElasticSearch.Client.ElasticSearch.Results;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Utils
{
    public static class AggregateResultExtensions
    {
        public static void PrintResult(this AggregateResult result)
        {
            Console.Out.WriteLine(JsonConvert.SerializeObject(result.ResultObject, Formatting.Indented));
        }
    }
}