using System;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Serializer.Converters
{
    public class AggregateItemConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((IAggregateComponent)value).BuildRequestComponent());
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IAggregateComponent).IsAssignableFrom(objectType);
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