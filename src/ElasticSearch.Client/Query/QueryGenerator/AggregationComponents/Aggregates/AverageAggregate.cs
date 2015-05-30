using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AverageAggregate : AggregateComponentBase
    {
        public AverageAggregate(string aggregateField)
            : base("avg")
        {
            Set(Field(aggregateField));
        }
    }
}