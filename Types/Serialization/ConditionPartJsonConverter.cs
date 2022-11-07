using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using Types.AlertContexts.LogAlertsV2;
using Types.AlertContexts;

namespace Types.Serialization
{
    public class ConditionPartJsonConverter : CovariantConverter<LogAlertsV2AlertContext, IConditionPart[]>
    {
        public override bool CanRead => true;
        public override LogAlertsV2AlertContext? ReadJson(JsonReader reader, Type objectType, LogAlertsV2AlertContext? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            //var variantName = new CamelCaseNamingStrategy().GetPropertyName(nameof(AlertContexts.LogAlertsV2.AlertContext.Condition), false);
            var variantPropertyPath = "condition.allOf";
            var (parent, variant) = InstantiateCovariant(obj, variantPropertyPath, ancestor => ancestor.ConditionType, variant => variant.Single().ConditionTypeMatch, serializer);

            Func<IConditionPart[], string[]> func = ooo => ooo.Single().ConditionTypeMatch;

            parent.Condition.AllOf = variant;

            return parent;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, LogAlertsV2AlertContext? value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
