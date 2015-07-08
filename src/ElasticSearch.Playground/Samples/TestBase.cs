using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.Query.QueryGenerator;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    public class TestBase
    {
        protected ElasticSearchClient Client;
        protected QueryBuilder QueryBuilder;

        public TestBase()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("einstein_engine-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            Client = new ElasticSearchClient("http://172.22.9.99:9200/", repSecIndex);
            QueryBuilder = new QueryBuilder();
        }

        [TearDown]
        public void TearDown()
        {
            QueryBuilder = new QueryBuilder();
        }
    }
}