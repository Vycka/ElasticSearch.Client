using System;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Groups
{
    public class TermGroup : IGroupComponent
    {
        private readonly string _fieldName;

        public TermGroup(string fieldName) 
        {
            if (fieldName == null) 
                throw new ArgumentNullException("fieldName");

            _fieldName = fieldName;
        }

        public string OperationName
        {
            get { return "terms"; }
        }

        public object BuildRequestComponent()
        {
            return new
            {
                    field = _fieldName
            };
        }
    }
}