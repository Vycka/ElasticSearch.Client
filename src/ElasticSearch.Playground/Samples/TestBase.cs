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
            var index = new TimeStampedIndexDescriptor("einstein_agency-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            Client = new ElasticSearchClient("http://10.8.58.65:9200/", index);
        }

        [SetUp]
        public void TearDown()
        {
            QueryBuilder = new QueryBuilder();
        }
    }
}