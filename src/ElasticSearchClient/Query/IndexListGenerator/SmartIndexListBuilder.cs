using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.ElasticSearch;
using ElasticSearch.Query.QueryGenerator;
using ElasticSearch.Query.QueryGenerator.Models;
using ElasticSearch.Query.QueryGenerator.QueryComponents;
using Newtonsoft.Json;

namespace ElasticSearch.Query.IndexListGenerator
{
    // It's Smart, because it analyses a query, and tries to optimize queried indexes to ES
    // Based on the requested timeframes specified in range queries
    public class SmartIndexListBuilder
    {
        private readonly ElasticSearchIndexDescriptor[] _indexDescriptors;
        private readonly QueryBuilder _filledQuery;

        public SmartIndexListBuilder(ElasticSearchIndexDescriptor[] indexDescriptors, QueryBuilder filledQuery)
        {
            if (indexDescriptors == null)
                throw new ArgumentNullException("indexDescriptors");
            if (filledQuery == null)
                throw new ArgumentNullException("filledQuery");

            _indexDescriptors = indexDescriptors;
            _filledQuery = filledQuery;
        }

        public string[] BuildLookupIndexes()
        {
            var lookupIndexes = new List<string>(2 * _indexDescriptors.Length);

            foreach (ElasticSearchIndexDescriptor indexDescriptor in _indexDescriptors)
            {
                if (indexDescriptor.IsAll == true)
                {
                    lookupIndexes.Add("_all");
                }
                else
                {
                    QueryDate queryDate = LookupBestQueryRange(indexDescriptor.IndexTimeStampField);

                    if (queryDate == null)
                    {
                        lookupIndexes.Add(indexDescriptor.IndexPrefix + "*");
                        System.Diagnostics.Trace.Write(
                            string.Format(
                                "WARNING: Index {0} contains no index count limiting filters\r\n{1}",
                                indexDescriptor.IndexPrefix,
                                JsonConvert.SerializeObject(_filledQuery.BuildRequestObject())
                            )
                        );
                    }
                    else
                    {
                        foreach (
                            DateTime shardTime in
                                IndexTimeStampGenerator.Generate(queryDate.RequestFrom, queryDate.RequestTo,
                                    indexDescriptor.IndexStep))
                        {
                            string lookupIndexName = indexDescriptor.IndexPrefix +
                                                     shardTime.ToString(indexDescriptor.IndexTimePattern);
                            lookupIndexes.Add(lookupIndexName);
                        }
                    }
                }

            }
            return lookupIndexes.ToArray();
        }

        private QueryDate LookupBestQueryRange(string fieldName)
        {
            var relatedQueries = _filledQuery.Filtered.Filters.Items
                .Where(
                    qd => 
                        qd.FilterComponent != null
                        && qd.FilterType != FilterType.MustNot
                        && qd.FilterComponent.GetQueryDate().FieldName == fieldName
                )
                .Select(f => f.FilterComponent.GetQueryDate())
                .ToList();

            if (relatedQueries.Count == 0)
                return null;

            DateTime minDate = relatedQueries.Min(rq => rq.RequestFrom);
            DateTime maxDate = relatedQueries.Max(rq => rq.RequestTo);

            return new QueryDate(fieldName, minDate, maxDate);
        }
    }
}