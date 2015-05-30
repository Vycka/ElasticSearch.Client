using System;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.Models.Ranges;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class RangeAggregate : AggregateComponentBase, IGroupComponent
    {
        public RangeAggregate(string aggregateFieldName, params Range[] ranges)
            : this(aggregateFieldName, false, ranges)
        {
        }

        public RangeAggregate(string aggregateFieldName, bool keyed, params Range[] ranges)
            : base("range")
        {
            if (aggregateFieldName == null)
                throw new ArgumentNullException("aggregateFieldName");

            ExpandoObject rangeRequest = new ExpandoObject();

            rangeRequest.Add("field", aggregateFieldName);
            rangeRequest.Add("keyed", keyed);
            rangeRequest.Add("ranges", ranges);

            Set(rangeRequest);
        }

    }
}