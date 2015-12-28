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
    public class SimpleQuery : TestBase
    {
        [Test]
        public void ExecuteTermQuery()
        {

            QueryBuilder builder = new QueryBuilder();

            builder.SetQuery(new LuceneQuery("_type:einstein_engine"));

            builder.PrintQuery(Client.IndexDescriptors);

            ElasticSearchResult result = Client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "einstein_engine"));
        }

        [Test]
        public void ExecuteMatchAllQuery()
        {

            QueryBuilder builder = new QueryBuilder();

            builder.SetQuery(new MatchAll());
            builder.Size = 1;

            builder.PrintQuery(Client.IndexDescriptors);

            ElasticSearchResult result = Client.ExecuteQuery(builder);

            Assert.AreEqual(1, result.Items.Count);
        }
    }
}