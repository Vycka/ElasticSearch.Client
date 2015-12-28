using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Queries
{
    [TestFixture]
    public class EmptyQuery : TestBase
    {
        [Test]
        public void ExpectDefaultCountReturned()
        {
            QueryBuilder builder = new QueryBuilder();

            builder.PrintQuery(Client.IndexDescriptors);

            ElasticSearchResult result = Client.ExecuteQuery(builder);

            Assert.AreEqual(10, result.Items.Count);
        }
    }
}