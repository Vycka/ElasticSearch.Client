namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class SumAggregate : AggregateComponentBase
    {
        public SumAggregate(string field)
            : base("sum")
        {
            SetOperationObject(Field(field));
        }
    }
}