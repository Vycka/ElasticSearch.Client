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

            ExpandoObject rangeRequest = new ExpandoObject();

            rangeRequest.Add("field", field);
            rangeRequest.Add("interval", interval);

            SetOperationObject(rangeRequest);
        }

        public string Format
        {
            get { return (string)GetFromOperationObject("format"); }
            set { UpdateOperationObject("format", value); }
        }

        public void SetTimeZoneOffset(int offset)
        {
            UpdateOperationObject("time_zone", offset);
        }

        public void SetTimeZoneOffset(string offsetFormatted)
        {
            UpdateOperationObject("time_zone", offsetFormatted);
        }
    }
}