namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class StatsAggregate : AggregateComponentBase
    {
        public StatsAggregate(string field)
            : base("stats")
        {
            Components.Set("field", field);
        }
    }
}