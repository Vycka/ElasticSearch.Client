using System.Linq;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
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

            Assert.AreEqual(10, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
        }
    }
}