using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.ElasticSearch.Results
{
    public class SearchResult<TResultModel>
    {
        public int Took { get; private set; }
        public bool TimedOut { get; private set; }
        public ShardResult Shards { get; private set; }

        public dynamic Aggregations { get; private set; }

        internal dynamic SearchResultObject;

        internal SearchResult(string searchResultJson)
        {
            //{"took":15,"timed_out":false,"_shards":{"total":4,"successful":4,"failed":0},"hits":{"total":0,"max_score":null,"hits":[]}}

            SearchResultObject = JsonConvert.DeserializeObject<dynamic>(searchResultJson);
            Aggregations = SearchResultObject.aggregations;

            Took = SearchResultObject.took;
            TimedOut = SearchResultObject.timed_out;

            Shards = new ShardResult(SearchResultObject._shards);


        }


        private List<TResultModel> _logItems; 
        public IReadOnlyList<TResultModel> Items
        {
            get
            {
                if (_logItems == null)
                    _logItems = new List<TResultModel>(
                        JsonConvert.DeserializeObject<List<TResultModel>>(
                            SearchResultObject.hits.hits.ToString()
                        )
                    );

                return _logItems.AsReadOnly();
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
