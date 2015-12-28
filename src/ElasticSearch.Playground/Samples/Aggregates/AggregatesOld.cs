using System;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Order;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Aggregates
{
    [TestFixture]
    public class AggregatesOld : TestBase
    {
        [Test]
        public void CountAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("county", new CountAggregate("TotalDuration"));
            builder.Aggregates.Add("valuy", new ValueCountAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.NotNull(result.GetValue("county.value"));
            Assert.AreEqual(result.GetValue<double>("county.value"), result.GetValue<double>("valuy.value"));
        }

        [Test]
        public void AverageAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new AverageAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            double resultValue = result.GetValue<double>("my_result.value");
            Assert.Greater(resultValue, 0.0);
        }

        [Test]
        public void MinAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new MinAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            dynamic resultDynamic = result;
            result.PrintResult();

            Assert.IsNotNull(result.GetValue<double>("my_result.value"));
            Assert.IsNotNull(resultDynamic.my_result.value);
        }

        [Test]
        public void MaxAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new MaxAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.Greater(result.GetValue<double>("my_result.value"), 0.0);
        }

        [Test]
        public void SumAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new SumAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.Greater(result.GetValue<double>("my_result.value"), 10000.0);
        }

        [Test]
        public void PercentilesAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("my_result", new PercentilesAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            double result25 = (double)result.GetValue<JObject>("my_result.values")["25.0"];
            double result50 = (double)result.GetValue<JObject>("my_result.values")["50.0"];

            Assert.Greater(result25, 0.0);
            Assert.Greater(result50, 0.0);
        }

        [Test]
        public void StatsAggregate()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must,
                new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));
            builder.Aggregates.Add("my_result", new StatsAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
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
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must,
                new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));
            builder.Aggregates.Add("my_result", new ExtendedStatsAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
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
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must,
                new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));
            builder.Aggregates.Add("some_sum", new SumAggregate("TotalDuration"));
            builder.Aggregates.Add("serious_min", new MinAggregate("TotalDuration"));
            builder.Aggregates.Add("heavy_stats", new StatsAggregate("TotalDuration"));

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            result.GetValue("serious_min.value");
            result.GetValue("some_sum.value");
            result.GetValue("heavy_stats.count");
        }

        [Test]
        public void TermsAggregates()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new FixedTimeRange("@timestamp", DateTime.UtcNow.Yesterday(), DateTime.UtcNow));

            builder.Aggregates.Add("my_term", new TermsAggregate("EventType", 10) { Order = new OrderField()});

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();


            Assert.Greater(result.GetValues<dynamic>("my_term.buckets").Length, 0);
        }
    }
}