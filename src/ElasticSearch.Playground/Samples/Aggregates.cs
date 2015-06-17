using System;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class Aggregates
    {
        [Test]
        public void CountAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("county", new CountAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("valuy", new ValueCountAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.NotNull(result.GetValue("county.value"));
            Assert.AreEqual(result.GetValue<double>("county.value"), result.GetValue<double>("valuy.value"));
        }

        [Test]
        public void AverageAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new AverageAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            double resultValue = result.GetValue<double>("my_result.value");
            Assert.Greater(resultValue, 0.0);
        }

        [Test]
        public void MinAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new MinAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            dynamic resultDynamic = result;
            result.PrintResult();

            Assert.AreEqual(0.0, result.GetValue<double>("my_result.value"));
            Assert.AreEqual(0.0, (double)resultDynamic.my_result.value);
        }

        [Test]
        public void MaxAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new MaxAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.Greater(result.GetValue<double>("my_result.value"), 0.0);
        }

        [Test]
        public void SumAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new SumAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.Greater(result.GetValue<double>("my_result.value"), 10000.0);
        }

        [Test]
        public void PercentilesAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new PercentilesAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            double result25 = (double)result.GetValue<JObject>("my_result.values")["25.0"];
            double result50 = (double)result.GetValue<JObject>("my_result.values")["50.0"];

            Assert.Greater(result25, 0.0);
            Assert.Less(result50, 100.0);
        }

        [Test]
        public void StatsAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must,
                new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));
            builder.Aggregates.Add("my_result", new StatsAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            result.GetValue("my_result.count");
            result.GetValue("my_result.min");
            result.GetValue("my_result.max");
            result.GetValue("my_result.avg");
            result.GetValue("my_result.sum");
        }

        [Test]
        public void ExtendedStatsAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must,
                new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));
            builder.Aggregates.Add("my_result", new ExtendedStatsAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            result.GetValue("my_result.count");
            result.GetValue("my_result.min");
            result.GetValue("my_result.max");
            result.GetValue("my_result.avg");
            result.GetValue("my_result.sum");

            result.GetValue("my_result.sum_of_squares");
            result.GetValue("my_result.variance");
            result.GetValue("my_result.std_deviation");
        }

        [Test]
        public void MultipleAggregates()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must,
                new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));
            builder.Aggregates.Add("some_sum", new SumAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("serious_min", new MinAggregate("Event.TotalDuration"));
            builder.Aggregates.Add("heavy_stats", new StatsAggregate("Event.TotalDuration"));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            result.GetValue("serious_min.value");
            result.GetValue("some_sum.value");
            result.GetValue("heavy_stats.count");
        }

        [Test]
        public void TermsAggregates()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));

            builder.Aggregates.Add("my_term", new TermsAggregate("Event.EventType", 10));

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();


            Assert.AreEqual(10, result.GetValues<dynamic>("my_term.buckets").Length);
        }
    }
}