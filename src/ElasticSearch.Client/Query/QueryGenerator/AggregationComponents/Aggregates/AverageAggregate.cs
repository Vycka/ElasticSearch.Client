namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AverageAggregate : AggregateComponentBase
    {
        public AverageAggregate(string field)
            : base("avg")
        {
            SetOperationObject(Field(field));
        }
    }
}