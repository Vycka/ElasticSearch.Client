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
    public class DateHistogram : TestBase
    {
        [Test]
        public void SimpleQuery()
        {
            QueryBuilder builder = new QueryBuilder();
            
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            DateHistogramAggregate aggregate = new DateHistogramAggregate("@timestamp", "6h");
            builder.Aggregates.Add("test", aggregate);


            builder.PrintQuery(_client.IndexDescriptors);

            AggregateResult result = _client.ExecuteAggregate(builder);
            dynamic resultDynamic = result;

            result.PrintResult();

            Assert.AreEqual(5, result.GetValues<object>("test.buckets").Length);
            Assert.AreEqual(5, ((JArray)resultDynamic.test.buckets).Count);
        }
    }
}