using System;
using System.Collections.Generic;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents
{
    public class AggregationQuery : IAggregateComponent
    {
        private readonly Dictionary<string, IAggregateComponent> _aggregateItems;

        public AggregationQuery()
        {
            _aggregateItems = new Dictionary<string, IAggregateComponent>();
        }

        public void Add(string aggregateName, IAggregateComponent aggregateComponent)
        {
            if (aggregateName == null)
                throw new ArgumentNullException("aggregateName");

            _aggregateItems.Add(aggregateName, aggregateComponent);
        }

        public IReadOnlyDictionary<string, IAggregateComponent> Items
        {
            get
            {
                return _aggregateItems;
            }
        }

        public object BuildRequestComponent()
        {
            if (Items.Count == 0)
                return null;

            return Items;
        }
    }
}