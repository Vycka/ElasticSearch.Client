using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Query.QueryGenerator.QueryComponents.Sort
{
    public class SortField : ISortComponent
    {
        private readonly string _fieldName;
        private readonly SortOrder _sortOrder;

        public SortField(string fieldName, SortOrder sortOrder = SortOrder.Asc)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            _fieldName = fieldName;
            _sortOrder = sortOrder;
        }

        public object BuildSortComponent()
        {
            var sortComponent = new ExpandoObject();

            ((IDictionary<string, object>)sortComponent).Add(_fieldName, new { order = _sortOrder.ToString().ToLowerInvariant(), ignore_unmapped = true });

            return sortComponent;
        }
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}