using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class PercentilesAggregate : AggregateComponentBase
    {
        public PercentilesAggregate(string aggregateField)
        {
            Add(AggregateType.Percentiles.GetName(), Field(aggregateField));
        }
    }
}