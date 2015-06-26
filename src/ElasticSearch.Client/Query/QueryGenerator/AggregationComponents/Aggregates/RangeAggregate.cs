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


            SetComponentProperty("field", field);

            Keyed = keyed;
            Ranges = ranges;
        }

        public bool? Keyed
        {
            get { return (bool?)GetComponentProperty("keyed"); }
            set { SetComponentProperty("keyed", value); }
        }

        public Range[] Ranges
        {
            get { return (Range[])GetComponentProperty("ranges"); }
            set { SetComponentProperty("ranges", value); }
        }
    }
}