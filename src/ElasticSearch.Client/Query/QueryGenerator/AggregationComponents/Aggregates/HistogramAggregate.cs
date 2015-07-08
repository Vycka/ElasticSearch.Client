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
            get { return Components.Get<string>("min_doc_count"); }
            set { Components.Set("min_doc_count", value); }
        }

        public string Interval
        {
            get { return Components.Get<string>("interval"); }
            set { Components.Set("interval", value); }
        }
    }
}