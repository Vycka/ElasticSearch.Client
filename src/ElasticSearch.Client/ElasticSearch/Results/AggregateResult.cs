using System;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace ElasticSearch.Client.ElasticSearch.Results
{
    /// <summary>
    /// AggregateResult can be cast to dynamic and used like:
    /// ((dynamic)result).field.subfield
    /// </summary>
    public class AggregateResult : DynamicObject
    {
        public readonly JObject ResultObject;

        internal AggregateResult(dynamic aggregateObject)
        {
            ResultObject = aggregateObject;
        }

        public object GetValue(string fieldPath)
        {
            return GetValue<object>(fieldPath);
        }

        public TValue GetValue<TValue>(string fieldPath)
        {
            return ResultObject.SelectToken(fieldPath, true).Value<TValue>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            try
            {
                result = GetValue(binder.Name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}