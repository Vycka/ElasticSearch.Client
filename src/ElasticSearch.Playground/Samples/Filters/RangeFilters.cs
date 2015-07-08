using System;
using ElasticSearch.Client.Query.IndexListGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Filters
{
    [TestFixture]
    public class RangeFilters : TestBase
    {
        [Test]
        public void CorrectSmartIndexBuilt()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new RangeFilter("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow.EndOfDay()));

            var indexBuilder = new SmartIndexListBuilder(Client.IndexDescriptors, QueryBuilder);
            Assert.AreEqual(2, indexBuilder.BuildLookupIndexes().Length);
        }

        [Test]
        public void CorrectAllIndexBuilt()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new RangeFilter("@timestamp", DateTime.UtcNow.Yesterday().ToString("O"), DateTime.UtcNow.EndOfDay()));

            var indexBuilder = new SmartIndexListBuilder(Client.IndexDescriptors, QueryBuilder);
            Assert.AreEqual("einstein_engine-*", indexBuilder.BuildLookupIndexes()[0]);
        }

        [Test]
        public void GteLteQuery()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new RangeFilter("@timestamp", DateTime.UtcNow.Yesterday().ToString("O"), DateTime.UtcNow.EndOfDay()));
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new RangeFilter("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow.EndOfDay()));


            QueryBuilder.PrintQuery(Client.IndexDescriptors);

            var result = Client.ExecuteQuery(QueryBuilder);

            Assert.AreEqual(10, result.Items.Count);
        }
    }
}
