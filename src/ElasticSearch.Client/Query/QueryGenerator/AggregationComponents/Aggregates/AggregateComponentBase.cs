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

        protected void Set(ExpandoObject operationValue)
        {
            _aggregateRequestValues.Add(_operationName, operationValue);
        }

        protected static ExpandoObject Field(string value)
        {
            var expandoObject = new ExpandoObject();
            expandoObject.Add("field",value);
            return expandoObject;
        }
    }
}