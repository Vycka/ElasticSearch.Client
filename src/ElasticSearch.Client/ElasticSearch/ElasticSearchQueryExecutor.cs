﻿using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;

namespace ElasticSearch.Client.ElasticSearch
{
    public class ElasticSearchQueryExecutor
    {
        private readonly HttpRequest _httpRequest;

        public ElasticSearchQueryExecutor(HttpRequest httpRequest)
        {
            if (httpRequest == null) throw new ArgumentNullException("httpRequest");
            _httpRequest = httpRequest;
        }


        public SearchResult<TResultModel> ExecuteQuery<TResultModel>(ElasticSearchQuery query, params GetParam[] additionalGetParams)
        {
            // First, try to execute query by quering all shards (try to save one http request for getting shard names)
            try
            {
                return ExecuteQueryInner<TResultModel>(query, additionalGetParams);
            }
            catch (Exception ex)
            {
                // If full request throws 404 error, then request list of existing shards
                if (ex.Message.Contains("(404) Not Found"))
                {
                    query = RemoveInexistingShards(query);

                    return ExecuteQueryInner<TResultModel>(query, additionalGetParams);
                }
                throw;
            }
        }

        public string ExecuteQueryRaw(ElasticSearchQuery query, params GetParam[] additionalGetParams)
        {
            // First, try to execute query by quering all shards (try to save one http request for getting shard names)
            try
            {
                return ExecuteQueryRawInner(query, additionalGetParams);
            }
            catch (Exception ex)
            {
                // If full request throws 404 error, then request list of existing shards
                if (ex.Message.Contains("(404) Not Found"))
                {
                    query = RemoveInexistingShards(query);

                    return ExecuteQueryRawInner(query, additionalGetParams);
                }
                throw;
            }
        }

        private SearchResult<TResultModel> ExecuteQueryInner<TResultModel>(ElasticSearchQuery query, params GetParam[] additionalGetParams)
        {
            string jsonResponse = ExecuteQueryRawInner(query, additionalGetParams);
            dynamic deserializedResponse = JsonConvert.DeserializeObject(jsonResponse);

            var result = new SearchResult<TResultModel>(deserializedResponse);

            if (result.TimedOut)
                throw new TimeoutException("There was a timeout while getting data from ElasticSearch");
            if (result.Shards.Total != 0 && result.Shards.Failed == result.Shards.Total)
                throw new Exception(string.Format("All shards returned failure [{0}]", BuildRequestUri(query, "_search", additionalGetParams)));

            return result;
        }

        private string ExecuteQueryRawInner(ElasticSearchQuery query, params GetParam[] additionalGetParams)
        {
            if (query.LookupIndexes.Length == 0)
                throw new NoShardsException(
                    "No matching indexes by provided query",
                    query
                );

            string requestUrl = BuildRequestUri(query, "_search", additionalGetParams);

            string jsonResponse = _httpRequest.MakePostJsonRequest(requestUrl, query.QueryJson);

            return jsonResponse;
        }

        private static string BuildRequestUri(ElasticSearchQuery query, string operationName, params GetParam[] additionalGetParams)
        {
            string additionalParams = "";
            if (additionalGetParams.Length != 0)
                additionalParams = "?" + String.Join("&", additionalGetParams.Select(p => String.Concat(p.Key, "=", p.Value)));

            string concatedShardNames = String.Join(",", query.LookupIndexes);
            string requestUrl = concatedShardNames + "/" + operationName + additionalParams;

            return requestUrl;
        }

        private ElasticSearchQuery RemoveInexistingShards(ElasticSearchQuery query)
        {
            string requestUrl = BuildRequestUri(query, "_aliases", new GetParam("ignore_unavailable", "true"));
                
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
        public readonly ElasticSearchQuery Query;

        public NoShardsException(string message, ElasticSearchQuery query)
            : base(message)
        {
            if (query == null) 
                throw new ArgumentNullException("query");

            Query = query;
        }
    }

    public class GetParam
    {
        public readonly string Key;
        public readonly string Value;

        public GetParam(string key, string value)
        {
            if (key == null) 
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");
            Key = key;
            Value = value;
        }
    }
}
