namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class ExtendedStatsAggregate : AggregateComponentBase
    {
        public ExtendedStatsAggregate(string field)
            : base("extended_stats")
        {
            SetOperationObject(Field(field));
        }
    }
}