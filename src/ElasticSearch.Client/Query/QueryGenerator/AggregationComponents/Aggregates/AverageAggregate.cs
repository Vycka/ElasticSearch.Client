using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AverageAggregate : AggregateComponentBase
    {
        public AverageAggregate(string aggregateField)
        {
            Add(AggregateType.Average.GetName(), Field(aggregateField));
        }
    }
}