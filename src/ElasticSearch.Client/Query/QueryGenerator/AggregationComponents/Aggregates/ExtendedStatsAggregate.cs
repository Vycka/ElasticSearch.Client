using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class ExtendedStatsAggregate : AggregateComponentBase
    {
        public ExtendedStatsAggregate(string aggregateField)
            : base("extended_stats")
        {
            Set(Field(aggregateField));
        }
    }
}