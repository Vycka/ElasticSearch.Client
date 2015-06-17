using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Aggregates
{
    [TestFixture]
    public class Histogram : TestBase
    {
        [Test]
        public void SimpleQuery()
        {
            QueryBuilder builder = new QueryBuilder();

            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            HistogramAggregate aggregate = new HistogramAggregate("TotalDuration", "1000");
            builder.Aggregates.Add("test", aggregate);


            builder.PrintQuery(_client.IndexDescriptors);

            AggregateResult result = _client.ExecuteAggregate(builder);
            dynamic resultDynamic = result;

            result.PrintResult();

            Assert.GreaterOrEqual(result.GetValues<object>("test.buckets").Length, 10);
            Assert.GreaterOrEqual(((JArray)resultDynamic.test.buckets).Count, 10);
        }
    }
}