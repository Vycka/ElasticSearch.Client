using System.Dynamic;
using ElasticSearch.Client.Query.Utils;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class TermsFilter : IFilterComponent
    {
        private readonly ComponentBag _termsFilterComponents = new ComponentBag();

        public TermsFilter(string key, params int[] value)
            : this(key, (object)value)
        {
        }

        public TermsFilter(string key, params string[] value)
            : this(key, (object)value)
        {
        }

        public TermsFilter(string key, object value)
        {
            _termsFilterComponents.Set(key, value);
        }

        public string Execution
        {
            get { return _termsFilterComponents.Get<string>("execution"); }
            set { _termsFilterComponents.Set("execution", value); }
        }

        public bool? Cache
        {
            get { return _termsFilterComponents.Get<bool?>("_cache"); }
            set { _termsFilterComponents.Set("_cache", value); }
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject result = new ExpandoObject();
            
            result.Add("terms", _termsFilterComponents);

            return result;
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}
