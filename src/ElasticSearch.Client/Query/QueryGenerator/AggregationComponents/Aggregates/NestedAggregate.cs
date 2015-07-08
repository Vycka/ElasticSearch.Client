namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class NestedAggregate : AggregateComponentBase, IGroupComponent
    {
        public NestedAggregate(string path) : base("nested")
        {
            Components.Set("path", path);
        }
    }
}