namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MinAggregate : AggregateComponentBase
    {
        public MinAggregate(string field)
            : base("min")
        {
            SetOperationObject(Field(field));
        }
    }
}