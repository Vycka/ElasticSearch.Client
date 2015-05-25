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
            SmartIndexListBuilder indexBuilder = new SmartIndexListBuilder(IndexDescriptors, filledQuery);

            string queryJson = _querySerializer.BuildJsonQuery(filledQuery);
            string[] queryInexes = indexBuilder.BuildLookupIndexes();

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, queryInexes);
            return _elasticSearchExecutor.ExecuteAggregateQuery(query);
        }


        //public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery, params string[] overrideExecuteIndexes)
        //{
        //    return new ElasticSearchResult(ExecuteQuery<ResultItem>(filledQuery, overrideExecuteIndexes).SearchResultObject.ToString());
        //}

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery)
        {
            return new ElasticSearchResult(ExecuteQuery<ResultItem>(filledQuery).SearchResultObject.ToString());
        }

        public SearchResult<TResultModel> ExecuteQuery<TResultModel>(QueryBuilder filledQuery)
        {
            var indexBuilder = new SmartIndexListBuilder(IndexDescriptors, filledQuery);

            string queryJson = _querySerializer.BuildJsonQuery(filledQuery);
            string[] queryInexes = indexBuilder.BuildLookupIndexes();

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, queryInexes);
            return _elasticSearchExecutor.ExecuteQuery<TResultModel>(query);
        }

        //public SearchResult<TResultModel> ExecuteQuery<TResultModel>(QueryBuilder filledQuery, params string[] overrideExecuteIndexes)
        //{
        //    string queryJson = _querySerializer.BuildJsonQuery(filledQuery);

        //    ElasticSearchQuery query = new ElasticSearchQuery(queryJson, overrideExecuteIndexes);
        //    return _elasticSearchExecutor.ExecuteQuery<TResultModel>(query);
        //}


    }
}