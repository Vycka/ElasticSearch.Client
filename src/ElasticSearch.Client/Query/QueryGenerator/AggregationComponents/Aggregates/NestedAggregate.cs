using System;
using System.Collections.Generic;
using ElasticSearch.Client.Query.QueryGenerator.SectionBuilders;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class NestedAggregate : IAggregateComponent
    {
        private readonly IGroupComponent _groupByAggregate;
        public readonly AggregationBuilder Aggregates = new AggregationBuilder();

        public NestedAggregate(IGroupComponent groupByAggregate)
        {
            if (groupByAggregate == null)
                throw new ArgumentNullException("groupByAggregate");

            _groupByAggregate = groupByAggregate;
        }

        public NestedAggregate(IGroupComponent groupByAggregate, string childAggregateName, IAggregateComponent childAggregateComponent)
            : this(groupByAggregate)
        {
            if (childAggregateName == null) throw new ArgumentNullException("childAggregateName");
            if (childAggregateComponent == null) throw new ArgumentNullException("childAggregateComponent");

            Aggregates.Add(childAggregateName, childAggregateComponent);
        }

        public string OperationName { get { return _groupByAggregate.OperationName; } }

        public object BuildRequestComponent()
        {
            var groupAggregateComponent = new Dictionary<string, object>((Dictionary<string, object>) _groupByAggregate.BuildRequestComponent());

            groupAggregateComponent.AddIfNotNull("aggs", Aggregates.BuildRequestComponent());

            return groupAggregateComponent;
        } 
    }
}