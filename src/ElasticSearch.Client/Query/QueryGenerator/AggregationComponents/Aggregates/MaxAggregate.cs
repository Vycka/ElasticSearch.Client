using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MaxAggregate : AggregateComponentBase
    {
        public MaxAggregate(string aggregateField)
        {
            Add(AggregateType.Max.GetName(), Field(aggregateField));
        }
    }
}