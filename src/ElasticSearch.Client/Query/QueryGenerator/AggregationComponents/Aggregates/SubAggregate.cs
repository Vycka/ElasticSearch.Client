using System;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.SectionBuilders;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class SubAggregate : IAggregateComponent
    {
        private readonly IGroupComponent _groupByAggregate;
        public readonly AggregationBuilder Aggregates = new AggregationBuilder();

        public SubAggregate(IGroupComponent groupByAggregate)
        {
            if (groupByAggregate == null)
                throw new ArgumentNullException("groupByAggregate");

            _groupByAggregate = groupByAggregate;
        }

        public SubAggregate(IGroupComponent groupByAggregate, string childAggregateName, IAggregateComponent childAggregateComponent)
            : this(groupByAggregate)
        {
            if (childAggregateName == null) throw new ArgumentNullException("childAggregateName");
            if (childAggregateComponent == null) throw new ArgumentNullException("childAggregateComponent");

            Aggregates.Add(childAggregateName, childAggregateComponent);
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject groupAggregateComponent = _groupByAggregate.BuildRequestComponent();
            groupAggregateComponent.AddIfNotNull("aggs", Aggregates.BuildRequestComponent());

            return groupAggregateComponent;
        } 
    }
}