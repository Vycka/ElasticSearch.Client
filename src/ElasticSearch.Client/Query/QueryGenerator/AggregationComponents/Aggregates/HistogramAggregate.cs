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

            ExpandoObject rangeRequest = new ExpandoObject();

            rangeRequest.Add("field", field);
            rangeRequest.Add("interval", interval);

            SetOperationObject(rangeRequest);
        }

        public string MinDocCount
        {
            get { return (string)GetFromOperationObject("min_doc_count"); }
            set { UpdateOperationObject("min_doc_count", value); }
        }
    }
}