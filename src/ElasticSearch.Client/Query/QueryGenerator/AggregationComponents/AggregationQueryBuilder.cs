using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents
{
    public class AggregationQueryBuilder
    {
        private readonly Dictionary<string, IAggregateComponent> _aggregateItems;

        public AggregationQueryBuilder()
        {
            _aggregateItems = new Dictionary<string, IAggregateComponent>();
        }

        public void Add(string name, IAggregateComponent aggregateComponent)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _aggregateItems.Add(name, aggregateComponent);
        }

        public IReadOnlyDictionary<string, IAggregateComponent> Items
        {
            get
            {
                return _aggregateItems;
            }
        }

        public object BuildRequestEntity()
        {
            if (Items.Count == 0)
                return null;

            return Items;
        }
    }
}