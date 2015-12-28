
using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.IndexListGenerator;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.IndexStepGenerator
{
    [TestFixture]
    public class MonthStepGeneratorTests : TestBase
    {
        [Test]
        public void TestMonthStep()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 8640000));
            builder.PrintQuery(Client.IndexDescriptors);

            IEnumerable<DateTime> resultTimeStamps = 
                IndexTimeStampGenerator.Generate(DateTime.UtcNow.AddMonths(-2).StartOfMonth(), DateTime.UtcNow.StartOfDay(), IndexStep.Month);



            ElasticSearchResult result = Client.ExecuteQuery(builder);
            Assert.Greater(result.Shards.Successful, resultTimeStamps.Count());
        }
    }
}
