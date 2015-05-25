using System.Collections.Generic;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AggregateComponentBase : IAggregateComponent
    {

        private readonly Dictionary<string, object> _aggregateRequestValues = new Dictionary<string, object>();

        public object BuildRequestComponent()
        {
            if (_aggregateRequestValues.Count == 0)
                return null;

            return _aggregateRequestValues;
        }

        protected void Add(string aggregateOperation, object value)
        {
            _aggregateRequestValues.Add(aggregateOperation, value);
        }

        protected static object Field(string value)
        {
            return new { field = value };
        }
    }
}