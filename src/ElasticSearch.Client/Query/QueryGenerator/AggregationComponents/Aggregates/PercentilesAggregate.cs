namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class PercentilesAggregate : AggregateComponentBase
    {
        public PercentilesAggregate(string field)
            : base("percentiles")
        {
            SetOperationObject(Field(field));
        }
    }
}