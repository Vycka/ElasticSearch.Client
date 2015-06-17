using System.Collections.Generic;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class MaxAggregate : AggregateComponentBase
    {
        public MaxAggregate(string aggregateField)
            : base("max")
        {
            SetOperationObject(Field(aggregateField));
        }
    }
}