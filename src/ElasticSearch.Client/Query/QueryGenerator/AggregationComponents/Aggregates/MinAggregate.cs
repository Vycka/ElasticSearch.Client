using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MinAggregate : AggregateComponentBase
    {
        public MinAggregate(string aggregateField)
        {
            Add(AggregateType.Min.GetName(), Field(aggregateField));
        }
    }
}