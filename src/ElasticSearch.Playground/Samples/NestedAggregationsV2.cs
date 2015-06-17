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
    public class NestedAggregationsV2
    {

        [Test]
        public void RangeGroup()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));

            var rangeAggregate = new RangeAggregate(
                    "Event.TotalDuration",
                    new Range(null, 100),
                    new Range(100, 500),
                    new Range(500, null)
            );
            //rangeAggregate.Sort.Add(new SortField("buckets.key"));

            var rangeGroup = new NestedAggregate(rangeAggregate);
            rangeGroup.Aggregates.Add("count", new CountAggregate("Event.TotalDuration"));
            rangeGroup.Aggregates.Add("avg", new AverageAggregate("Event.TotalDuration"));

            builder.Aggregates.Add("range_group", rangeGroup);

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            result.PrintResult();

            Assert.IsNotNull(result.GetValue("range_group.buckets"));
        }


    }
}