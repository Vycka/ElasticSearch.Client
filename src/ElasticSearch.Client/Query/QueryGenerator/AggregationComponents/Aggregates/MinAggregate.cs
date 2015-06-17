namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MinAggregate : AggregateComponentBase
    {
        public MinAggregate(string aggregateField)
            : base("min")
        {
            SetOperationObject(Field(aggregateField));
        }
    }
}