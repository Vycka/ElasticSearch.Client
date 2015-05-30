using System;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Serializer.Converters
{
    public class SortItemConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((ISortComponent)value).BuildRequestComponent());
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ISortComponent).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}