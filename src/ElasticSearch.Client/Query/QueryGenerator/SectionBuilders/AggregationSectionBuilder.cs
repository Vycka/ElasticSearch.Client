using System;
using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.SectionBuilders
{
    public class AggregationBuilder
    {
        private readonly ExpandoObject _aggregateItems;

        public AggregationBuilder()
        {
            _aggregateItems = new ExpandoObject();
        }

        public void Add(string aggregateName, IAggregateComponent aggregateComponent)
        {
            if (aggregateName == null)
                throw new ArgumentNullException("aggregateName");

            _aggregateItems.Add(aggregateName, aggregateComponent);
        }


        public ExpandoObject BuildRequestComponent()
        {
            if (((IDictionary<string, object>)_aggregateItems).Count == 0)
                return null;

            return _aggregateItems;
        }
    }
}