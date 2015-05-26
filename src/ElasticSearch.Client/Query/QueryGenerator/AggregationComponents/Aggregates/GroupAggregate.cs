using System;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class GroupAggregate : IAggregateComponent
    {
        private readonly IGroupComponent _groupComponent;
        public readonly AggregationQuery Aggregates = new AggregationQuery(); 

        public GroupAggregate(IGroupComponent groupComponent)
        {
            if (groupComponent == null)
                throw new ArgumentNullException("groupComponent");

            _groupComponent = groupComponent;
        }

        public GroupAggregate(IGroupComponent groupComponent, string aggregationName, IAggregateComponent aggregateComponent)
            : this(groupComponent)
        {
            if (aggregationName == null) throw new ArgumentNullException("aggregationName");
            if (aggregateComponent == null) throw new ArgumentNullException("aggregateComponent");
            
            Aggregates.Add(aggregationName, aggregateComponent);
        }

        public object BuildRequestComponent()
        {
            ExpandoObject result = new ExpandoObject();

            result.Add(_groupComponent.OperationName, _groupComponent.BuildRequestComponent());
            result.AddIfNotNull("aggs", Aggregates.BuildRequestComponent());

            return result;
        }
    }
}