using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.Utils
{
    public class ComponentBag : IRequestComponent
    {
        private readonly ExpandoObject _components = new ExpandoObject();

         public void Set(string key, object value)
         {
             if (value == null)
                 ((IDictionary<string, object>)_components).Remove(key);
             else
                 _components.AddOrUpdate(key, value);
         }

         public T Get<T>(string key)
         {
             object result;
             ((IDictionary<string, object>)_components).TryGetValue(key, out result);
             return (T)result;
         }

        public int Count
        {
            get { return ((IDictionary<string, object>) _components).Count; }
        }

        public ExpandoObject BuildRequestComponent()
        {
            return _components;
        }
    }
}