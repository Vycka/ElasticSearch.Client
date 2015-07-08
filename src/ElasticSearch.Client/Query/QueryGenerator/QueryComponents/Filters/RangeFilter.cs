using System;
using System.Dynamic;
using ElasticSearch.Client.Query.Utils;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class RangeFilter : IFilterComponent
    {
        private readonly string _fieldName;
        
        private readonly ComponentBag _rangeFilterComponents = new ComponentBag();

        public RangeFilter(string fieldName, string gteValue, string lteValue = null)
            : this(fieldName, (object)gteValue, lteValue)
        {
        }

        public RangeFilter(string fieldName, int? gteValue, int? lteValue = null)
            : this(fieldName, (object)gteValue, lteValue)
        {
        }

        public RangeFilter(string fieldName, DateTime? gteValue, DateTime? lteValue = null)
            : this(fieldName, (object)gteValue, lteValue)
        {
        }

        public RangeFilter(string fieldName, object gteValue, object lteValue = null)
            : this(fieldName)
        {
            GreaterThanEqual = gteValue;
            LessThanEqual = lteValue;
        }

        public RangeFilter(string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException("fieldName");
            _fieldName = fieldName;
        }

        public object LessThanEqual
        {
            get { return _rangeFilterComponents.Get<object>("lte"); }
            set { _rangeFilterComponents.Set("lte", value); }
        }

        public object GreaterThanEqual
        {
            get { return _rangeFilterComponents.Get<object>("gte"); }
            set { _rangeFilterComponents.Set("gte", value); }
        }

        public object LessThan
        {
            get { return _rangeFilterComponents.Get<object>("lt"); }
            set { _rangeFilterComponents.Set("lt", value); }
        }

        public object GreaterThan
        {
            get { return _rangeFilterComponents.Get<object>("gt"); }
            set { _rangeFilterComponents.Set("gt", value); }
        }

        public object TimeZone
        {
            get { return _rangeFilterComponents.Get<object>("time_zone"); }
            set { _rangeFilterComponents.Set("time_zone", value); }
        }
        public string Format
        {
            get { return _rangeFilterComponents.Get<string>("format"); }
            set { _rangeFilterComponents.Set("format", value); }
        }

        public ExpandoObject BuildRequestComponent()
        {
            var result = new ExpandoObject();
            var rangeQuery = new ExpandoObject();

            rangeQuery.Add(_fieldName, _rangeFilterComponents);

            result.Add("range", rangeQuery);

            return result;
        }

        public QueryDate GetQueryDate()
        {
            DateTime? lteDate = LessThanEqual as DateTime?;
            DateTime? gteDate = GreaterThanEqual as DateTime?;

            QueryDate result = null;
            if (lteDate != null && gteDate != null)
                result = new QueryDate(_fieldName, gteDate.Value, lteDate.Value);

            return result;
        }
    }
}