namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class StatsAggregate : AggregateComponentBase
    {
        public StatsAggregate(string field)
            : base("stats")
        {
            SetOperationObject(Field(field));
        }
    }
}