using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.ElasticSearch
{    
    public class ElasticSearchResult
    {
        public int Took { get; private set; }
        public bool TimedOut { get; private set; }
        public ShardResult Shards { get; private set; }

        private readonly List<ResultItem> _logItems; 

        internal ElasticSearchResult(string jsonElasticSearchResponse)
        {
            //{"took":15,"timed_out":false,"_shards":{"total":4,"successful":4,"failed":0},"hits":{"total":0,"max_score":null,"hits":[]}}
            var logsParsed = JsonConvert.DeserializeObject<dynamic>(jsonElasticSearchResponse);

            Took = logsParsed.took;
            TimedOut = logsParsed.timed_out;

            Shards = new ShardResult(logsParsed._shards);

            _logItems = new List<ResultItem>(
                JsonConvert.DeserializeObject<List<ResultItem>>(
                    logsParsed.hits.hits.ToString()
                )
            );
        }

        public IReadOnlyList<ResultItem> Items
        {
            get
            {
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
