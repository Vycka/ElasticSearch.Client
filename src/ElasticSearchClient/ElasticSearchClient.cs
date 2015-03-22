using System;
using ElasticSearch.ElasticSearch;
using ElasticSearch.Query.IndexListGenerator;
using ElasticSearch.Query.QueryGenerator;
using ElasticSearch.Utils;
using Newtonsoft.Json;

namespace ElasticSearch
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

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery)
        {
            var indexBuilder = new SmartIndexListBuilder(_indexDescriptors, filledQuery);

            string queryJson = BuildJsonQuery(filledQuery);
            string[] queryInexes = indexBuilder.BuildLookupIndexes();

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, queryInexes);
            return _elasticSearchExecutor.ExecuteQuery(query);
        }

        public ElasticSearchResult ExecuteQuery(QueryBuilder filledQuery, params string[] executeOnIndexes)
        {
            string queryJson = BuildJsonQuery(filledQuery);

            ElasticSearchQuery query = new ElasticSearchQuery(queryJson, executeOnIndexes);
            return _elasticSearchExecutor.ExecuteQuery(query);
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