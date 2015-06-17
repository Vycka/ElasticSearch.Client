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

        public dynamic ExecuteAggregate(QueryBuilder filledQuery, bool countOnly = true)
        {
            ElasticSearchQuery query = BuildQuery(filledQuery);
            SearchResult<ResultItem> result;
            if (countOnly)
                result = _elasticSearchExecutor.ExecuteQuery<ResultItem>(query, new GetParam("search_type", "count"));
            else
                result = _elasticSearchExecutor.ExecuteQuery<ResultItem>(query);

            return result.Aggregations;
        }

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery)
        {
            return new ElasticSearchResult(ExecuteQuery<ResultItem>(filledQuery).SearchResultObject);
        }

        public SearchResult<TResultModel> ExecuteQuery<TResultModel>(QueryBuilder filledQuery)
        {
            ElasticSearchQuery query = BuildQuery(filledQuery);
            return _elasticSearchExecutor.ExecuteQuery<TResultModel>(query);
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