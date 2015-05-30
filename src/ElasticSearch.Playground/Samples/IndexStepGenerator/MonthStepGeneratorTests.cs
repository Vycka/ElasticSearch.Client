
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
    public class MonthStepGeneratorTests
    {
        [Test]
        public void TestMonthStep()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.*", "@timestamp", IndexStep.Month);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 8640000));
            builder.PrintQuery(client.IndexDescriptors);

            IEnumerable<DateTime> resultTimeStamps = 
                IndexTimeStampGenerator.Generate(DateTime.UtcNow.AddMonths(-2).StartOfMonth(), DateTime.UtcNow.StartOfDay(), IndexStep.Month);



            ElasticSearchResult result = client.ExecuteQuery(builder);
            Assert.Greater(result.Shards.Successful, resultTimeStamps.Count());
        }
    }
}
