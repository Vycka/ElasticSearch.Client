using System;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class DateHistogramAggregate : AggregateComponentBase
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
            get { return (string)GetComponentProperty("format"); }
            set { SetComponentProperty("format", value); }
        }

        public void SetTimeZoneOffset(int offset)
        {
            SetComponentProperty("time_zone", offset);
        }

        public void SetTimeZoneOffset(string offsetFormatted)
        {
            SetComponentProperty("time_zone", offsetFormatted);
        }

        public string Interval
        {
            get { return (string)GetComponentProperty("interval"); }
            set { SetComponentProperty("interval", value); }
        }
    }
}