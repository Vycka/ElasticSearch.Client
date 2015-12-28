using System;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using ElasticSearch.Playground.Samples;
using NUnit.Framework;

namespace ElasticSearch.Playground
{
    [TestFixture]
    public class FixedTimeRangeTest : TestBase
    {
        [Test]
        public void FixedModifiebleTimeRange()
        {
            FixedTimeRange timeFilter = new FixedTimeRange("@timestamp");
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, timeFilter);

            QueryBuilder.PrintQuery();

            timeFilter.UtcFrom = DateTime.UtcNow.AddDays(-1);
            timeFilter.UtcTo = DateTime.UtcNow;

            QueryBuilder.PrintQuery();

            var result = Client.ExecuteQueryCount(QueryBuilder);

            Assert.Greater(result.Total, 0);
        }
    }
}