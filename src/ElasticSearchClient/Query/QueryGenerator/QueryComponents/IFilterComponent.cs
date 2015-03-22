using System;

namespace ElasticSearch.Query.QueryGenerator.QueryComponents
{
    public interface IFilterComponent
    {
        object BuildFilterComponent();

        /// <summary>
        /// Tell about its timerange filter properites, so request index space will not execute ES query on all available indices
        /// Return null if it doesn't apply to that filter
        /// </summary>
        /// <returns></returns>
        QueryDate GetQueryDate();

    }

    public class QueryDate
    {
        public readonly string FieldName;
        public readonly DateTime RequestFrom;
        public readonly DateTime RequestTo;

        public QueryDate(string fieldName, DateTime requestFrom, DateTime requestTo)
        {
            if (fieldName == null) throw new ArgumentNullException("fieldName");
            FieldName = fieldName;
            RequestFrom = requestFrom;
            RequestTo = requestTo;
        }
    }
}