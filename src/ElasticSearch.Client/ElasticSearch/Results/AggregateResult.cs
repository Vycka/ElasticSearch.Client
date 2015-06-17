using System;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ElasticSearch.Client.ElasticSearch.Results
{
    /// <summary>
    /// AggregateResult can be cast to dynamic and used like:
    /// ((dynamic)result).field.subfield
    /// </summary>
    public class AggregateResult : DynamicObject
    {
        public readonly dynamic ResultObject;

        internal AggregateResult(dynamic aggregateObject)
        {
            ResultObject = aggregateObject;
        }

        public dynamic GetValue(string fieldPath)
        {
            return GetValue<dynamic>(fieldPath);
        }

        public TValue GetValue<TValue>(string fieldPath)
        {
            return ((JObject)ResultObject).SelectToken(fieldPath, true).Value<TValue>();
        }

        public dynamic[] GetValues(string fieldPath)
        {
            return GetValues<dynamic>(fieldPath);
        }

        public TValue[] GetValues<TValue>(string fieldPath)
        {
            return ((JObject)ResultObject).SelectToken(fieldPath, true).Values<TValue>().ToArray();
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