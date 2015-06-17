namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class NestedPathAggregate : AggregateComponentBase, IGroupComponent
    {
        public NestedPathAggregate(string path) : base("nested")
        {
            SetOperationObject(Field("path", path));
        }
    }
}