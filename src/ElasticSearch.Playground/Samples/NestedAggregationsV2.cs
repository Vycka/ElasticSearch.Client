using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.Models.Ranges;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class NestedAggregationsV2 : TestBase
    {

        [Test]
        public void RangeGroup()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var rangeAggregate = new RangeAggregate(
                    "TotalDuration",
                    new Range(null, 100),
                    new Range(100, 500),
                    new Range(500, null)
            );

            var rangeGroup = new SubAggregate(rangeAggregate);
            rangeGroup.Aggregates.Add("count", new CountAggregate("TotalDuration"));
            rangeGroup.Aggregates.Add("avg", new AverageAggregate("TotalDuration"));

            builder.Aggregates.Add("range_group", rangeGroup);

            builder.PrintQuery(Client.IndexDescriptors);

            AggregateResult result = Client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.IsNotNull(result.GetValue("range_group.buckets"));
        }


    }
}
