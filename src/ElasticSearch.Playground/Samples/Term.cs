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
    public class Term : TestBase
    {

        [Test]
        public void QueryByTerm()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new TermFilter("_type","einstein_engine"));

            QueryBuilder.PrintQuery(Client.IndexDescriptors);

            ElasticSearchResult result = Client.ExecuteQuery(QueryBuilder);

            Assert.AreEqual(10, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "einstein_engine"));
        }
    }
}