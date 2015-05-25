using System;
using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.IndexListGenerator;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;

namespace ElasticSearch.Client
{
    public class ElasticSearchClient
    {
        private readonly ElasticSearchIndexDescriptor[] _indexDescriptors;
        private readonly ElasticSearchQueryExecutor _elasticSearchExecutor;



        public ElasticSearchClient(string elasticSearchServiceUrl, params ElasticSearchIndexDescriptor[] indexDescriptors)
        {
            _indexDescriptors = indexDescriptors;
            if (elasticSearchServiceUrl == null)
                throw new ArgumentNullException("elasticSearchServiceUrl");
            if (indexDescriptors == null)
                throw new ArgumentNullException("indexDescriptors");

            HttpRequest httpRequest = new HttpRequest(elasticSearchServiceUrl);

            _elasticSearchExecutor = new ElasticSearchQueryExecutor(httpRequest);
        }

        public dynamic ExecuteAggregate(QueryBuilder filledQuery)
        {
            SmartIndexListBuilder indexBuilder = new SmartIndexListBuilder(_indexDescriptors, filledQuery);

            string queryJson = BuildJsonQuery(filledQuery);
            string[] queryInexes = indexBuilder.BuildLookupIndexes();

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, queryInexes);
            return _elasticSearchExecutor.ExecuteAggregateQuery(query);
        }

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery)
        {
            return new ElasticSearchResult(ExecuteQuery<ResultItem>(filledQuery).SearchResultObject.ToString());
        }

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery, params string[] executeOnIndexes)
        {
            return new ElasticSearchResult(ExecuteQuery<ResultItem>(filledQuery, executeOnIndexes).SearchResultObject.ToString());
        }

        public SearchResult<TResultModel> ExecuteQuery<TResultModel>(QueryBuilder filledQuery)
        {
            var indexBuilder = new SmartIndexListBuilder(_indexDescriptors, filledQuery);

            string queryJson = BuildJsonQuery(filledQuery);
            string[] queryInexes = indexBuilder.BuildLookupIndexes();

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, queryInexes);
            return _elasticSearchExecutor.ExecuteQuery<TResultModel>(query);
        }

        public SearchResult<TResultModel> ExecuteQuery<TResultModel>(QueryBuilder filledQuery, params string[] executeOnIndexes)
        {
            string queryJson = BuildJsonQuery(filledQuery);

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, executeOnIndexes);
            return _elasticSearchExecutor.ExecuteQuery<TResultModel>(query);
        }

        private static string BuildJsonQuery(QueryBuilder filledQuery)
        {
            string queryJson = JsonConvert.SerializeObject(
                filledQuery.BuildRequestObject(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                }
            );

            return queryJson;
        }
    }
}