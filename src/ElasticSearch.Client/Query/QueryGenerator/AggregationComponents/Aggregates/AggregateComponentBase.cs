using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AggregateComponentBase : IAggregateComponent
    {
        private readonly string _operationName;
        private readonly Dictionary<string, object> _aggregateRequestValues = new Dictionary<string, object>();

        public AggregateComponentBase(string operationName)
        {
            _operationName = operationName;
        }

        public string OperationName { get { return _operationName; } }

        public object BuildRequestComponent()
        {
            if (_aggregateRequestValues.Count == 0)
                return null;

            return _aggregateRequestValues;
        }

        protected void AddSubItem(string itemName, object subItemValue)
        {
            ((ExpandoObject)_aggregateRequestValues[_operationName]).Add(itemName, subItemValue);
        }

        private ExpandoObject OperationObject
        {
            get { return (ExpandoObject)_aggregateRequestValues[_operationName]; }
        }

        protected void SetOperationObject(ExpandoObject operationValue)
        {
            _aggregateRequestValues.Add(_operationName, operationValue);
        }

        protected static ExpandoObject Field(string value)
        {
            var expandoObject = new ExpandoObject();
            expandoObject.Add("field",value);
            return expandoObject;
        }

        public string Script
        {
            get { return (string)GetFromOperationObject("script"); }
            set { UpdateOperationObject("script", value); }
        }

        public int Size
        {
            get { return (int)GetFromOperationObject("size"); }
            set { UpdateOperationObject("size", value); }
        }

        protected void UpdateOperationObject(string key, object value)
        {
            OperationObject.AddOrUpdate(key, value);
        }

        protected object GetFromOperationObject(string key)
        {
            object result;
            ((IDictionary<string, object>)OperationObject).TryGetValue("script", out result);
            return result;
        }

        protected static ExpandoObject Field(string fieldName, string fieldValue)
        {
            var expandoObject = new ExpandoObject();
            expandoObject.Add(fieldName, fieldValue);
            return expandoObject;
        }
    }
}