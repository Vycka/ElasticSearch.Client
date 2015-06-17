using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class TermsAggregate : AggregateComponentBase, IGroupComponent
    {
        public TermsAggregate()
            : this(null)
        {
        }

        public TermsAggregate(string field, int? size = null) 
            : base("terms")
        {
            ExpandoObject termsRequest = new ExpandoObject();

            termsRequest.AddIfNotNull("field", field);
            termsRequest.AddIfNotNull("size", size);

            SetOperationObject(termsRequest);
        }

        private List<ISortComponent> _sortComponents;
        public List<ISortComponent> Sort
        {
            get
            {
                if (_sortComponents == null)
                {
                    _sortComponents = new List<ISortComponent>();
                    AddSubItem("order", _sortComponents);
                }

                return _sortComponents;
            }
        }
    }
}