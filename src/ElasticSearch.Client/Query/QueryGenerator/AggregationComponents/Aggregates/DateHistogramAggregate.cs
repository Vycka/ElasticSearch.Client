using System;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class DateHistogramAggregate : AggregateComponentBase, IGroupComponent
    {
        public DateHistogramAggregate(string field, string interval)
            : base("date_histogram")
        {
            if (field == null)
                throw new ArgumentNullException("field");
            if (interval == null)
                throw new ArgumentNullException("interval");

            Field = field;
            Interval = interval;
        }

        public string Format
        {
            get { return Components.Get<string>("format"); }
            set { Components.Set("format", value); }
        }

        public void SetTimeZoneOffset(int offset)
        {
            Components.Set("time_zone", offset);
        }

        public void SetTimeZoneOffset(string offsetFormatted)
        {
            Components.Set("time_zone", offsetFormatted);
        }

        public string Interval
        {
            get { return Components.Get<string>("interval"); }
            set { Components.Set("interval", value); }
        }
    }
}