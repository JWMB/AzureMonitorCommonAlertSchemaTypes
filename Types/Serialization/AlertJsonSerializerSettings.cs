using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;

namespace Types.Serialization
{
    public class AlertJsonSerializerSettings : JsonSerializerSettings
    {
        public AlertJsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Converters = new JsonConverter[]
            {
                new AlertDataJsonConverter(), 
                new ConditionPartJsonConverter()
            };
        }

        public static Alert? Deserialize(string data)
        {
            var alert = JsonConvert.DeserializeObject<Alert>(data, new AlertJsonSerializerSettings());
            return alert;
        }
        public static Alert DeserializeOrThrow(string data)
        {
            var alert = JsonConvert.DeserializeObject<Alert>(data, new AlertJsonSerializerSettings());
            if (alert == null)
                throw new Exception("Could not deserialize data");
            return alert;
        }
    }
}
