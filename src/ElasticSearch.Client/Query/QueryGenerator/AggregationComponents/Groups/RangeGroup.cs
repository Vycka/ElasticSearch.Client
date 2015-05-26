using System;
using ElasticSearch.Client.Query.QueryGenerator.Models.Ranges;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Groups
{
    public class RangeGroup : IGroupComponent
    {
        private readonly string _fieldName;
        private readonly Range[] _ranges;

        public RangeGroup(string fieldName, params Range[] ranges)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            _fieldName = fieldName;
            _ranges = ranges;
        }

        public string OperationName
        {
            get { return "range"; }
        }

        public object BuildRequestComponent()
        {
            return new
            {
                field = _fieldName,
                ranges = _ranges
            };
        }
    }
}