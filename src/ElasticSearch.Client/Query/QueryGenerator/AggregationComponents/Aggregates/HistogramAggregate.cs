using System;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class HistogramAggregate : AggregateComponentBase
    {
        public HistogramAggregate(string field, string interval)
            : base("histogram")
        {
            if (field == null)
                throw new ArgumentNullException("field");
            if (interval == null)
                throw new ArgumentNullException("interval");

            Field = field;
            Interval = interval;
        }

        public string MinDocCount
        {
            get { return (string)GetComponentProperty("min_doc_count"); }
            set { SetComponentProperty("min_doc_count", value); }
        }

        public string Interval
        {
            get { return (string)GetComponentProperty("interval"); }
            set { SetComponentProperty("interval", value); }
        }
    }
}