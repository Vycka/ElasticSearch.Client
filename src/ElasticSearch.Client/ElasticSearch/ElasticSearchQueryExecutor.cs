using System;
using System.Collections.Generic;
using ElasticSearch.Playground.Utils;
using Newtonsoft.Json;

namespace ElasticSearch.Playground.ElasticSearch
{
    public class ElasticSearchQueryExecutor
    {
        private readonly HttpRequest _httpRequest;

        public ElasticSearchQueryExecutor(HttpRequest httpRequest)
        {
            if (httpRequest == null) throw new ArgumentNullException("httpRequest");
            _httpRequest = httpRequest;
        }

        public ElasticSearchResult ExecuteQuery(ElasticSearchQuery query)
        {
            // First, try to execute query by quering all shards (try to save one http request for getting shard names)
            try
            {
                return ExecuteQueryInner(query);
            }
            catch (Exception ex)
            {
                // If full request throws 404 error, then request list of existing shards
                if (ex.Message.Contains("(404) Not Found"))
                {
                    query = RemoveInexistingShards(query);

                    return ExecuteQueryInner(query);
                }
                throw;
            }
        }

        private ElasticSearchResult ExecuteQueryInner(ElasticSearchQuery query)
        {
            if (query.LookupIndexes.Length == 0)
                throw new NoShardsException(
                    string.Format("Not even a single shard overlaps with requested time period!")
                );

            string concatedShardNames = String.Join(",", query.LookupIndexes);
            string requestUrl = concatedShardNames + "/_search";

            string jsonResponse = _httpRequest.MakePostJsonRequest(requestUrl, query.QueryJson);

            var result = new ElasticSearchResult(jsonResponse);

            if (result.TimedOut)
                throw new TimeoutException("There was a timeout while getting data from ElasticSearch");
            if (result.Shards.Failed != 0 && result.Shards.Total != 0)
                throw new Exception(string.Format("One or more shards returned failure [{0}]", concatedShardNames));

            return result;
        }

        private ElasticSearchQuery RemoveInexistingShards(ElasticSearchQuery query)
        {
            string concatedShardNames = String.Join(",", query.LookupIndexes);
            string requestUrl = concatedShardNames + "/_aliases?ignore_missing=true";

            string jsonResponse = _httpRequest.MakeGetRequest(requestUrl);
            var parsedResponse = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonResponse);

            List<string> existingShards = new List<string>(query.LookupIndexes.Length);
            foreach (string shardName in parsedResponse.Keys)
            {
                existingShards.Add(shardName);
            }
            
            return new ElasticSearchQuery(query.QueryJson, existingShards.ToArray());
        }
    }

    public class NoShardsException : Exception
    {
        public NoShardsException(string message) : base(message)
        {
        }
    }
}
