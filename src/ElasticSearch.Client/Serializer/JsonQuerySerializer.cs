using System.IO;
using System.Text;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Serializer.Converters;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Serializer
{
    public class JsonQuerySerializer
    {
        private readonly JsonSerializer _jsonSerializer = CreateJsonSerializer();

        public bool PrettyPrint
        {
            get
            {
                return _jsonSerializer.Formatting == Formatting.Indented; 
            }
            set
            {
                _jsonSerializer.Formatting = value ? Formatting.Indented : Formatting.None;
            }
        }

        public string BuildJsonQuery(QueryBuilder filledQuery)
        {
            var result = new StringBuilder();

            using (var textWriter = new StringWriter(result))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                _jsonSerializer.Serialize(jsonWriter, filledQuery.BuildRequestObject());
            }

            return result.ToString();
        }

        private static JsonSerializer CreateJsonSerializer()
        {
            JsonSerializer serializer = JsonSerializer.Create();

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new ObjectDictionaryConverter());
            serializer.Converters.Add(new AggregateItemConverter());

            return serializer;
        }
    }
}