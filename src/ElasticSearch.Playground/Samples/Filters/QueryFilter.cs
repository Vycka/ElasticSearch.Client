using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Filters
{
    public class QueryFilter : TestBase
    {
        [Test]
        public void QueryFilterTest()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",86400));
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new LuceneFilter("Level:Info"));

            QueryBuilder.PrintQuery(Client.IndexDescriptors);

            var result = Client.ExecuteQuery(QueryBuilder);

            Assert.AreEqual(10, result.Items.Count);
        }
    }
}