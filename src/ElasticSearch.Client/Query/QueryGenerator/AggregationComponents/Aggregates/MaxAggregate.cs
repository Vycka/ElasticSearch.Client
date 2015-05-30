using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MaxAggregate : AggregateComponentBase
    {
        public MaxAggregate(string aggregateField)
            : base("max")
        {
            Set(Field(aggregateField));
        }
    }
}