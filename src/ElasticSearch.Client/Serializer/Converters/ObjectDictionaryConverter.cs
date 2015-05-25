using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Serializer.Converters
{
    public class ObjectDictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            var input = (IEnumerable<KeyValuePair<string, object>>)value;
            foreach (var keypair in input)
            {
                writer.WritePropertyName(keypair.Key);
                serializer.Serialize(writer, keypair.Value);
            }
            writer.WriteEndObject();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<KeyValuePair<string, object>>).IsAssignableFrom(objectType);
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