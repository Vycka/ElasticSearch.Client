using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Sort
{
    public class SortField : ISortComponent
    {
        private readonly string _fieldName;
        private readonly bool _ignoreUnmapped;
        private readonly SortOrder _sortOrder;

        public SortField(string fieldName, bool ignoreUnmapped = true, SortOrder sortOrder = SortOrder.Asc)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            _fieldName = fieldName;
            _ignoreUnmapped = ignoreUnmapped;
            _sortOrder = sortOrder;
        }

        public ExpandoObject BuildRequestComponent()
        {
            var sortComponent = new ExpandoObject();

            ((IDictionary<string, object>)sortComponent).Add(_fieldName, new { order = _sortOrder.ToString().ToLowerInvariant(), ignore_unmapped = _ignoreUnmapped });

            return sortComponent;
        }
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}