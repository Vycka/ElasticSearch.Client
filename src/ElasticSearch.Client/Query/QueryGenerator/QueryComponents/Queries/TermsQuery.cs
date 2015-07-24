using System.Dynamic;
using ElasticSearch.Client.Query.Utils;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries
{
    public class TermsQuery : IQueryComponent
    {
        private readonly ComponentBag _termsQueryComponents = new ComponentBag();

        public TermsQuery(string key, params int[] value)
            : this(key, (object)value)
        {
        }

        public TermsQuery(string key, params string[] value)
            : this(key, (object)value)
        {
        }

        public TermsQuery(string key, object value)
        {
            _termsQueryComponents.Set(key, value);
        }

        public int? MinimumShouldMatch
        {
            get { return _termsQueryComponents.Get<int>("minimum_should_match"); }
            set { _termsQueryComponents.Set("minimum_should_match", value); }
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject result = new ExpandoObject();
            
            result.Add("terms", _termsQueryComponents);

            return result;
        }
    }
}