using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class TermsAggregate : AggregateComponentBase, IGroupComponent
    {
        public TermsAggregate(string aggregateField, int? size = null) 
            : base("terms")
        {
            ExpandoObject termsRequest = new ExpandoObject();

            termsRequest.Add("field", aggregateField);
            termsRequest.AddIfNotNull("size", size);

            Set(termsRequest);
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