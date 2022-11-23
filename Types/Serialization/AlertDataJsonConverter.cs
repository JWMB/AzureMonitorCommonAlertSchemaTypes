using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace AzureMonitorCommonAlertSchemaTypes.Serialization
{
    public class AlertDataJsonConverter : CovariantConverter<Data, IAlertContext>
    {
        public override bool CanRead => true;
        public override Data? ReadJson(JsonReader reader, Type objectType, Data? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var variantName = new CamelCaseNamingStrategy().GetPropertyName(nameof(Data.AlertContext), false);
            var (parent, variant) = InstantiateCovariant(obj, variantName, ancestor => ancestor.Essentials.MonitoringService, variant => variant.MonitoringServiceMatches, serializer);

            parent.AlertContext = variant!;

            return parent;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, Data? value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
