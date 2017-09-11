using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Aggregates
{

    [TestFixture]
    public class PercentilesAggregateTest : TestBase
    {
        [Test]
        public void Percentiles()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400*7));

            QueryBuilder.Aggregates.Add("95",
                new SubAggregate(
                    new TermsAggregate("Request.ConsumerInfo.id"),
                    "consumer",
                    new PercentilesAggregate("TotalDuration", new []{ 95.0 })
                )
            );

            QueryBuilder.Aggregates.Add("99",
                new SubAggregate(
                    new TermsAggregate("Request.ConsumerInfo.id"),
                    "consumer",
                    new PercentilesAggregate("TotalDuration", new[] { 99.0 })
                )
            );

            AggregateResult result = Client.ExecuteAggregate(QueryBuilder);

 /*
"99": {
    "doc_count_error_upper_bound": 0,
    "sum_other_doc_count": 75080,
    "buckets": [
        {
        "key": "foo",
        "doc_count": 326120,
        "consumer": {
            "values": {
            "99.0": 1.23
            }
        }
    },
    ...
}
             */

            Assert.Greater(result.GetValues<dynamic>("99.buckets").Length, 0);
            Assert.Greater(result.GetValues<dynamic>("95.buckets").Length, 0);
        }

        
    }
}