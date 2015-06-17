Trying to create friendly, yet functional __ElasticSearch C# Client__.

__Currently supported__
* Basic Query section support (Useful for retrieving raw data)
  * Query, Filter
  * Lucene, Term, Indices, TimeRange
* Aggregations
  * Average, ExtendedStats, Min, Max, Percentiles, Stats, Sum, ValueCount, Terms, DateHistogram, Histogram
* Group By
  * Range, Term
  
___See Playground (Tests?) project for more usage examples___

___Need a feature? Write  issue with a feature request___

__You can execute queries as simple as these__

```cs
  ElasticSearchClient client = new ElasticSearchClient("http://localhost:9200/");

  QueryBuilder builder = new QueryBuilder();
  builder.SetQuery(new TermQuery("_type", "errors");

  ElasticSearchResult result = client.ExecuteQuery(builder);
```

__Or as complex as those__

```cs
  var rabbitIndex = new TimeStampedIndexDescriptor("RabbitMQ-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
  var securityIndex = new TimeStampedIndexDescriptor("Security-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
  ElasticSearchClient client = new ElasticSearchClient("http://localhost:9200/", rabbitIndex, securityIndex);

  QueryBuilder builder = new QueryBuilder();
  builder.Filtered.Queries.Add(QueryType.Should, new TermQuery("Level","ERROR"));
  builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
  builder.Filtered.Filters.Add(FilterType.MustNot, new LuceneFilter("EventType:IrrelevantError"));

  ElasticSearchResult result = client.ExecuteQuery(builder);
```

And as of the result, QueryBuilder in the current release build will generate query looking like this:

```js
POST: http://localhost:9200/
                          RabbitMQ-2015.03.21,
                          RabbitMQ-2015.03.22,
                          Security-2015.03.21,
                          Security-2015.03.22
                          /_search
{
  "query": {
    "filtered": {
      "query": {
        "bool": {
          "should": [
            {
              "term": {
                "Level": "ERROR"
              }
            }
          ]
        }
      },
      "filter": {
        "bool": {
          "must": [
            {
              "range": {
                "@timestamp": {
                  "from": "now-86400s",
                  "to": "now"
                }
              }
            }
          ],
          "must_not": [
            {
              "fquery": {
                "query": {
                  "query_string": {
                    "query": "EventType:IrrelevantError"
                  }
                },
                "_cache": true
              }
            }
          ]
        }
      }
    }
  },
  "size": 500
}
```

ElasticSearch DB query-wise is very feature-rich, so it takes a lot of work to represent it all in QueryBuilder.
Because of that, it only supports mostly those features which I added, when I needed them.

But if you are missing a feature: contact me, or branch it; develop it; and merge request it :)
