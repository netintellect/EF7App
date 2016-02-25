using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServiceCruiser.Model.Entities.Converters
{
    public class CustomJsonCreateConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var jObject = JObject.Load(reader);

            T target = Create(objectType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        private T Create(Type objectType, JObject jObject)
        {
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (jObject == null) throw new ArgumentNullException("jObject");

            JToken token;
            jObject.TryGetValue("$type", out token);

            if (token != null)
            {
                //Remove the assembly name from type name
                var typeFullName = token.ToString().Split(',').First();

                //First figure out if the type is a derived type from the base (objectType)
                var type = objectType.Assembly.GetTypes().FirstOrDefault(t => t.BaseType == objectType && t.FullName == typeFullName);

                //Then try to locate the type in the Executing assembly
                if (type == null)
                    type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.FullName == typeFullName);

                //If it is still not found, try to get the type from AssemblyQualifiedName
                if (type == null)
                    type = Type.GetType(token.ToString(), false);

                if (type != null)
                    return (T)Activator.CreateInstance(type);

                throw new NotSupportedException(string.Format("CustomCreationConverter cannot create an instance for {0}", token));
            }
            throw new NotSupportedException("CustomCreationConverter cannot create an instance (no 'type' token found)");

        }
    }
}
