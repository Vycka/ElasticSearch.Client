using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ElasticSearch.Client.ElasticSearch.Results
{
    public class SearchResult<TResultModel>
    {
        public int Took { get; private set; }
        public bool TimedOut { get; private set; }
        public ShardResult Shards { get; private set; }

        internal dynamic SearchResultObject;

        internal SearchResult(dynamic searchResult)
        {
            // {"took":15,"timed_out":false,"_shards":{"total":4,"successful":4,"failed":0},"hits":{"total":0,"max_score":null,"hits":[]}}

            SearchResultObject = searchResult;

            Took = SearchResultObject.took;
            TimedOut = SearchResultObject.timed_out;

            Shards = new ShardResult(SearchResultObject._shards);
        }

        // Items
        private List<ResultItem<TResultModel>> _logItems;
        public IReadOnlyList<ResultItem<TResultModel>> Items
        {
            get
            {
                if (_logItems == null)
                {
                    JArray resultArray = SearchResultObject.hits.hits;
                    IEnumerable<ResultItem<TResultModel>> resultDeserialized = resultArray.Select(r => ((JObject) r).ToObject<ResultItem<TResultModel>>());
                    _logItems = new List<ResultItem<TResultModel>>(resultDeserialized);
                }

                return _logItems;
            }
        }

        public int? Total
        {
            get { return SearchResultObject.hits.total; }
        }

        public double? MaxScore
        {
            get { return SearchResultObject.hits.max_score; }
        }

        // Aggregations
        private AggregateResult _aggregations;
        public AggregateResult Aggregations
        {
            get
            {
                if (_aggregations == null && SearchResultObject.aggregations != null)
                    _aggregations = new AggregateResult(SearchResultObject.aggregations);

                return _aggregations;
            }
        }

        public class ShardResult
        {
            public ShardResult(dynamic jsonShardInfo)
            {
                Total = jsonShardInfo.total;
                Successful = jsonShardInfo.successful;
                Failed = jsonShardInfo.failed;
            }

            public int Total { get; private set; }
            public int Successful { get; private set; }
            public int Failed { get; private set; }
        }
    }
}
