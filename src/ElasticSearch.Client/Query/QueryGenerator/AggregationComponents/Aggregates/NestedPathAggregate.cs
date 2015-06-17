namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class NestedPathAggregate : AggregateComponentBase, IGroupComponent
    {
        public NestedPathAggregate(string nestingPath) : base("nested")
        {
            SetOperationObject(Field("path", nestingPath));
        }
    }
}