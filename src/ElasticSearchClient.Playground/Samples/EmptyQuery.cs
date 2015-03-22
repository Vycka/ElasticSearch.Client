using ElasticSearchClient.ElasticSearch;
using ElasticSearchClient.ElasticSearch.Index;
using ElasticSearchClient.Playground.Utils;
using ElasticSearchClient.Query.QueryGenerator;
using NUnit.Framework;

namespace ElasticSearchClient.Playground.Samples
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

            Assert.AreEqual(500, result.Items.Count);
        }
    }
}