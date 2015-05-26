using System;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Groups;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.Models.Ranges;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class AggregateWithGroup
    {
        [Test]
        public void GroupByTerm()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var groupQuery = new GroupAggregate(new TermGroup("Event.EventType"), "some_stats", new StatsAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("my_stats_group", groupQuery);

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.my_stats_group.buckets);
        }

        [Test]
        public void GroupByRange()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var groupQuery = new GroupAggregate(new RangeGroup("Event.TotalDuration", new Range(0,10), new Range(10,20)), "some_stats", new CountAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("my_stats_group", groupQuery);

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.my_stats_group.buckets);
        }

        [Test]
        public void DoubleGroup()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var termGroup = new GroupAggregate(new TermGroup("Event.EventType"), "some_stats", new StatsAggregate("Event.TotalDuration"));
            var rangeGroup = new GroupAggregate(new RangeGroup("Event.TotalDuration", new Range(0, 10), new Range(10, 20)), "some_stats", new CountAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("term_group", termGroup);
            builder.Aggregates.Add("range_group", rangeGroup);

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.range_group.buckets);
        }

        [Test]
        public void DoubleAggregateGroup()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var rangeGroup = new GroupAggregate(
                new RangeGroup(
                    "Event.TotalDuration", 
                    new Range(0, 10),
                    new Range(10, 20),
                    new Range(30, 40)
                )
            );
            rangeGroup.Aggregates.Add("min", new MinAggregate("Event.TotalDuration"));
            rangeGroup.Aggregates.Add("max", new MaxAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("range_group", rangeGroup);

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.range_group.buckets);
        }
    }
}