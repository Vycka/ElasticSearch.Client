using System;
using System.Collections.Generic;
using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.IndexListGenerator;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Serializer;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client
{
    public class ElasticSearchClient
    {
        public readonly List<ElasticSearchIndexDescriptor> IndexDescriptors;
        private readonly ElasticSearchQueryExecutor _elasticSearchExecutor;

        private readonly JsonQuerySerializer _querySerializer = new JsonQuerySerializer();

        public ElasticSearchClient(string elasticSearchServiceUrl, params ElasticSearchIndexDescriptor[] indexDescriptors)
        {
            
            if (elasticSearchServiceUrl == null)
                throw new ArgumentNullException("elasticSearchServiceUrl");
            if (indexDescriptors == null)
                throw new ArgumentNullException("indexDescriptors");

            IndexDescriptors = new List<ElasticSearchIndexDescriptor>(indexDescriptors);
            HttpRequest httpRequest = new HttpRequest(elasticSearchServiceUrl);
            _elasticSearchExecutor = new ElasticSearchQueryExecutor(httpRequest);
        }

        public dynamic ExecuteAggregate(QueryBuilder filledQuery)
        {
            ElasticSearchQuery query = BuildQuery(filledQuery);
            var result = _elasticSearchExecutor.ExecuteQuery<dynamic>(query, new GetParam("search_type", "count"));
            return result.Aggregations;
        }

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery, params GetParam[] additionGetParams)
        {
            return new ElasticSearchResult(ExecuteQuery<dynamic>(filledQuery, additionGetParams).SearchResultObject);
        }

        /// <summary>
        /// Executes query and returns original JSON response.
        /// Usefull when custom deserializer is needed or deserialization is not required 
        /// </summary>
        public string ExecuteQueryRaw(QueryBuilder filledQuery, params GetParam[] additionGetParams)
        {
            ElasticSearchQuery query = BuildQuery(filledQuery);
            return _elasticSearchExecutor.ExecuteQueryRaw(query, additionGetParams);
        }

        public SearchResult<TResultModel> ExecuteQuery<TResultModel>(QueryBuilder filledQuery, params GetParam[] additionGetParams)
        {
            ElasticSearchQuery query = BuildQuery(filledQuery);
            return _elasticSearchExecutor.ExecuteQuery<TResultModel>(query, additionGetParams);
        }

        private ElasticSearchQuery BuildQuery(QueryBuilder filledQuery)
        {
            var indexBuilder = new SmartIndexListBuilder(IndexDescriptors, filledQuery);

            string queryJson = _querySerializer.BuildJsonQuery(filledQuery);
            string[] queryInexes = indexBuilder.BuildLookupIndexes();

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, queryInexes);

            return query;
        }
    }
}