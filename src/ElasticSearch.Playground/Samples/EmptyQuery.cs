using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class EmptyQuery
    {
        [Test]
        public void ExpectDefaultCountReturned()
        {
            ElasticSearchIndexDescriptor repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();

            builder.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreEqual(10, result.Items.Count);
        }
    }
}