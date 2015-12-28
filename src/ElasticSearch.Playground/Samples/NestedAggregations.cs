using System;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.Models.Ranges;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class NestedAggregations : TestBase
    {
        [Test]
        public void GroupByTerm()
        {

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var groupQuery = new SubAggregate(new TermsAggregate("EventType"), "some_stats", new StatsAggregate("TotalDuration"));
            builder.Aggregates.Add("my_stats_group", groupQuery);

            builder.PrintQuery(Client.IndexDescriptors);

            dynamic result = Client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.my_stats_group.buckets);
        }

        [Test]
        public void GroupByRange()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var groupQuery = new SubAggregate(new RangeAggregate("TotalDuration", new Range(0, 10), new Range(10, 20)), "some_stats", new CountAggregate("TotalDuration"));
            builder.Aggregates.Add("my_stats_group", groupQuery);

            builder.PrintQuery(Client.IndexDescriptors);

            dynamic result = Client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.my_stats_group.buckets);
        }

        [Test]
        public void DoubleGroup()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var termGroup = new SubAggregate(new TermsAggregate("EventType"), "some_stats", new StatsAggregate("TotalDuration"));
            var rangeGroup = new SubAggregate(new RangeAggregate("TotalDuration", new Range(0, 10), new Range(10, 20)), "some_stats", new CountAggregate("TotalDuration"));
            builder.Aggregates.Add("term_group", termGroup);
            builder.Aggregates.Add("range_group", rangeGroup);

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.IsNotNull(result.GetValue("range_group.buckets"));
        }

        [Test]
        public void DoubleAggregateGroup()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var rangeGroup = new SubAggregate(
                new RangeAggregate(
                    "TotalDuration", 
                    new Range(null, 10),
                    new Range(10, 20),
                    new Range(30, 40)
                )
            );
            rangeGroup.Aggregates.Add("min", new MinAggregate("TotalDuration"));
            rangeGroup.Aggregates.Add("max", new MaxAggregate("TotalDuration"));
            builder.Aggregates.Add("range_group", rangeGroup);

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.IsNotNull(result.GetValue("range_group.buckets"));
        }
    }
}