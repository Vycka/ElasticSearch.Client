using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;

namespace ElasticSearch.Playground.Samples
{
    public class TestBase
    {
        protected ElasticSearchClient _client;

        public TestBase()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("einstein_engine-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            _client = new ElasticSearchClient("http://172.22.9.99:9200/", repSecIndex);
        } 
    }
}