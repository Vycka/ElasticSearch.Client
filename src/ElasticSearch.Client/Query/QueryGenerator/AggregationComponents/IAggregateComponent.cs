namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents
{
    public interface IAggregateComponent : IRequestComponent
    {
        string OperationName { get; }
    }
}