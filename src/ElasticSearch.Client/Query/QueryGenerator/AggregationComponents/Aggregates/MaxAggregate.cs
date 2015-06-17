namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MaxAggregate : AggregateComponentBase
    {
        public MaxAggregate(string field)
            : base("max")
        {
            SetOperationObject(Field(field));
        }
    }
}