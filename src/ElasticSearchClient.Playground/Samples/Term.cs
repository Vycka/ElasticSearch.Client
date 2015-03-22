using System.Linq;
using ElasticSearchClient.ElasticSearch;
using ElasticSearchClient.Playground.Utils;
using ElasticSearchClient.Query.QueryGenerator;
using ElasticSearchClient.Query.QueryGenerator.Models;
using ElasticSearchClient.Query.QueryGenerator.QueryComponents.Filters;
using NUnit.Framework;

namespace ElasticSearchClient.Playground.Samples
{
    [TestFixture]
    public class Term
    {

        [Test]
        public void QueryByTerm()
        {
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/");
            QueryBuilder builder = new QueryBuilder();

            builder.Filtered.Filters.Add(FilterType.Must, new TermFilter("_type","rep-sec"));

            builder.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreEqual(500, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
        }
    }
}