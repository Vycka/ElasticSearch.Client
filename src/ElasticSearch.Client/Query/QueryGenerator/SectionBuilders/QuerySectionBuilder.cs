using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.SectionBuilders
{
    public class QueryListBuilder
    {
        public List<QueryItem> Items = new List<QueryItem>();

        public void Add(QueryType queryType, IQueryComponent queryComponent)
        {
            Items.Add(new QueryItem(queryType, queryComponent));
        }

        public void Add(QueryItem queryItem)
        {
            Items.Add(queryItem);
        }

        internal ExpandoObject BuildQueryBoolRequest()
        {
            ExpandoObject requestObject = new ExpandoObject();
            foreach (QueryType queryType in Enum.GetValues(typeof(QueryType)))
            {
                QueryType localQueryType = queryType;
                var queryObjects = Items
                    .Where(q => q.QueryType == localQueryType)
                    .Select(qq => qq.QueryComponent.BuildRequestComponent()).ToList();

                if (queryObjects.Count != 0)
                    requestObject.Add(QueryTypeMapping.GetName(queryType), queryObjects);
            }
            if (requestObject.GetItems().Count != 0)
                return requestObject;

            return null;
        }
    }
}