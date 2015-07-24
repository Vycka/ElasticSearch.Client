using System.Linq;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Queries
{

    [TestFixture]
    public class SimpleQuery
    {
        [Test]
        public void ExecuteTermQuery()
        {

            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/");

            QueryBuilder builder = new QueryBuilder();

            builder.SetQuery(new LuceneQuery("_type:rep-sec"));

            builder.PrintQuery(client.IndexDescriptors);

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
        }

        [Test]
        public void ExecuteMatchAllQuery()
        {

            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/");

            QueryBuilder builder = new QueryBuilder();

            builder.SetQuery(new MatchAll());
            builder.Size = 1;

            builder.PrintQuery(client.IndexDescriptors);

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreEqual(1, result.Items.Count);
        }
    }
}