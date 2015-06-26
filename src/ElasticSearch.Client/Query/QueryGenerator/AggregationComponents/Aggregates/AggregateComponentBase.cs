using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AggregateComponentBase : IAggregateComponent
    {
        private readonly string _aggregateOperationName;
        private readonly Dictionary<string, object> _aggregateOperationComponent = new Dictionary<string, object>();

        public AggregateComponentBase(string aggregateOperationName)
        {
            _aggregateOperationName = aggregateOperationName;
        }

        public ExpandoObject BuildRequestComponent()
        {
            if (_aggregateOperationComponent.Count == 0)
                return null;

            ExpandoObject requestComponent = new ExpandoObject();
            requestComponent.Add(_aggregateOperationName, _aggregateOperationComponent);

            return requestComponent;
        }

        protected string Field
        {
            get { return (string)GetComponentProperty("field"); }
            set { SetComponentProperty("field", value); }
        }

        public string Script
        {
            get { return (string)GetComponentProperty("script"); }
            set { SetComponentProperty("script", value); }
        }

        public int? Size
        {
            get { return (int?)GetComponentProperty("size"); }
            set { SetComponentProperty("size", value); }
        }

        protected void SetComponentProperty(string key, object value)
        {
            if (value == null)
                _aggregateOperationComponent.Remove(key);
            else
                _aggregateOperationComponent.AddOrUpdate(key, value);
        }

        protected object GetComponentProperty(string key)
        {
            object result;
            _aggregateOperationComponent.TryGetValue("script", out result);
            return result;
        }
    }
}