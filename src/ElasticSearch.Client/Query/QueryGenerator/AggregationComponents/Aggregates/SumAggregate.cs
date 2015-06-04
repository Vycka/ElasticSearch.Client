using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class SumAggregate : AggregateComponentBase
    {
        public SumAggregate(string aggregateField)
            : base("sum")
        {
            SetOperationObject(Field(aggregateField));
        }
    }
}