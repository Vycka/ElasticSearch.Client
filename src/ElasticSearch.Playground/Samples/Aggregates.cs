using System;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using ElasticSearch.Playground.Utils;
using Newtonsoft.Json;
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

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.NotNull(result.county.value);
            Assert.AreEqual(result.county.value, result.valuy.value);
        }

        [Test]
        public void AverageAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new AverageAggregate("Event.TotalDuration"));

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            double resultValue = result.my_result.value;
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

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            double resultValue = result.my_result.value;
            Assert.AreEqual(0.0, resultValue);
        }

        [Test]
        public void MaxAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new MaxAggregate("Event.TotalDuration"));

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            double resultValue = result.my_result.value;
            Assert.Greater(resultValue, 0.0);
        }

        [Test]
        public void SumAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new SumAggregate("Event.TotalDuration"));

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            double resultValue = result.my_result.value;
            Assert.Greater(resultValue, 10000.0);
        }

        [Test]
        public void PercentilesAggregate()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new PercentilesAggregate("Event.TotalDuration"));

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            var resultValue = (Newtonsoft.Json.Linq.JObject) result.my_result.values;

            Assert.Greater(resultValue.Value<double>("25.0"), 0.0);
            Assert.Less(resultValue.Value<double>("50.0"), 100.0);
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

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.NotNull(result.my_result.count);
            Assert.NotNull(result.my_result.min);
            Assert.NotNull(result.my_result.max);
            Assert.NotNull(result.my_result.avg);
            Assert.NotNull(result.my_result.sum);
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

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.NotNull(result.my_result.count);
            Assert.NotNull(result.my_result.min);
            Assert.NotNull(result.my_result.max);
            Assert.NotNull(result.my_result.avg);
            Assert.NotNull(result.my_result.sum);

            Assert.NotNull(result.my_result.sum_of_squares);
            Assert.NotNull(result.my_result.variance);
            Assert.NotNull(result.my_result.std_deviation);
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

            builder.PrintQuery();

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.NotNull(result.serious_min.value);
            Assert.NotNull(result.some_sum.value);
            Assert.NotNull(result.heavy_stats.count);
        }
    }
}