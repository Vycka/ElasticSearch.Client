﻿using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;

namespace ElasticSearch.Client.Query.IndexListGenerator
{
    // It's Smart, because it analyses a query, and tries to optimize queried indexes to ES
    // Based on the requested timeframes specified in range queries
    public class SmartIndexListBuilder
    {
        private readonly List<ElasticSearchIndexDescriptor> _indexDescriptors;
        private readonly QueryBuilder _filledQuery;

        public SmartIndexListBuilder(List<ElasticSearchIndexDescriptor> indexDescriptors, QueryBuilder filledQuery)
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
            var lookupIndexes = new List<string>(2 * _indexDescriptors.Count);

            foreach (ElasticSearchIndexDescriptor indexDescriptor in _indexDescriptors)
            {
                QueryDate queryDate = LookupBestQueryRange(indexDescriptor.IndexTimeStampField);

                if (queryDate == null || queryDate.FieldName != indexDescriptor.IndexTimeStampField)
                {
                    lookupIndexes.AddRange(indexDescriptor.GetIndexDescriptors());
                }
                else
                {
                    lookupIndexes.AddRange(indexDescriptor.GetIndexDescriptors(queryDate.RequestFrom, queryDate.RequestTo));
                }
            }

            if (lookupIndexes.Count == 0)
                lookupIndexes.Add("_all");

            return lookupIndexes.ToArray();
        }

        private QueryDate LookupBestQueryRange(string fieldName)
        {
            var relatedQueries = _filledQuery.Filtered.Filters.Items
                .Where(
                    qd => 
                        qd.FilterType != FilterType.MustNot
                )
                .Select(rqFiltered => rqFiltered.FilterComponent.GetQueryDate())
                .Where(queryDate => queryDate != null && queryDate.FieldName == fieldName)
                .ToList();

            if (relatedQueries.Count == 0)
                return null;

            DateTime minDate = relatedQueries.Min(rq => rq.RequestFrom);
            DateTime maxDate = relatedQueries.Max(rq => rq.RequestTo);

            return new QueryDate(fieldName, minDate, maxDate);
        }
    }
}