using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class StatsAggregate : AggregateComponentBase
    {
        public StatsAggregate(string aggregateField)
            : base("stats")
        {
            SetOperationObject(Field(aggregateField));
        }
    }
}