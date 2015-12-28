using System;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Indexes
{
    [TestFixture]
    public class ConcreteIndex : TestBase
    {
        [Test]
        public void TestConcreteIndex()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.PrintQuery(Client.IndexDescriptors);

            ElasticSearchResult result = Client.ExecuteQuery(builder);

            Assert.AreEqual(1, result.Shards.Successful);
        }
    }
}