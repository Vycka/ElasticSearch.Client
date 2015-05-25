using ElasticSearch.Client.Query.QueryGenerator.Models;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class ValueCountAggregate : AggregateComponentBase
    {
        public ValueCountAggregate(string aggregateField)
        {
            Add(AggregateType.ValueCount.GetName(), Field(aggregateField));
        }
    }

    public class CountAggregate : ValueCountAggregate
    {
        public CountAggregate(string aggregateField) : base(aggregateField)
        {
        }
    }

}