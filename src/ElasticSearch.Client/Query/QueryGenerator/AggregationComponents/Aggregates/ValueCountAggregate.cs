namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class ValueCountAggregate : AggregateComponentBase
    {
        public ValueCountAggregate(string field)
            : base("value_count")
        {
            SetOperationObject(Field(field));
        }
    }

    public class CountAggregate : ValueCountAggregate
    {
        public CountAggregate(string field) : base(field)
        {
        }
    }

}