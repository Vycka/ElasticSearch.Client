using System;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.Models.Ranges;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class RangeAggregate : AggregateComponentBase, IGroupComponent
    {
        public RangeAggregate(string field, params Range[] ranges)
            : this(field, false, ranges)
        {
        }

        public RangeAggregate(string field, bool keyed, params Range[] ranges)
            : base("range")
        {
            if (field == null)
                throw new ArgumentNullException("field");

            ExpandoObject rangeRequest = new ExpandoObject();

            rangeRequest.Add("field", field);
            rangeRequest.Add("keyed", keyed);
            rangeRequest.Add("ranges", ranges);

            SetOperationObject(rangeRequest);
        }

    }
}