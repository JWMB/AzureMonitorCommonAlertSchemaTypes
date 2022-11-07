using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Types.AlertContexts.LogAlertsV2;
using Types.AlertContexts;

namespace Types
{
    public class AlertJsonSerializerSettings : JsonSerializerSettings
    {
        public AlertJsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Converters = new JsonConverter[] { new AlertDataJsonConverter(), new PlatformConditionJsonConverter() };
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

    public abstract class CovariantConverter<TAncestorWithCovariantTypeId, TVariant> : JsonConverter<TAncestorWithCovariantTypeId>
    {
        private Dictionary<string, Type>? idToType;
        private void InitializeIdToType(Func<TVariant, string[]> getVariantNameMatches)
        {
            var baseType = typeof(TVariant);
            var isArray = baseType.IsArray;
            if (isArray)
            {
                baseType = baseType.GetElementType();
            }
            var types = baseType.Assembly.GetExportedTypes()
                .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract);

            idToType = types
                .Select(t =>
                {
                    if (isArray)
                    {
                        var inst = Activator.CreateInstance(t);
                        var arr = Array.CreateInstance(t, 1);
                        arr.SetValue(inst, 0);
                        return (TVariant)(object)arr!;
                    }
                    else
                        return (TVariant)Activator.CreateInstance(t);
                })
                .SelectMany(inst => getVariantNameMatches(inst).Select(name => new { Name = name, Type = inst!.GetType() }))
                .ToDictionary(o => o.Name, o => o.Type);
        }

        protected (TAncestorWithCovariantTypeId parent, TVariant variant) InstantiateCovariant(
            JObject obj, string variantPropertyPath, Func<TAncestorWithCovariantTypeId, string> getVariantNameFromAncestor, Func<TVariant, string[]> getVariantNameMatches, JsonSerializer serializer)
        {
            if (idToType == null)
                InitializeIdToType(getVariantNameMatches);

            var variantNode = obj.SelectToken(variantPropertyPath);
            if (variantNode == null)
                throw new ArgumentNullException($"{variantPropertyPath} is missing");

            variantNode.Parent!.Remove();

            var parent = obj.ToObject<TAncestorWithCovariantTypeId>();
            if (parent == null)
                throw new SerializationException($"Couldn't deserialize '{typeof(TAncestorWithCovariantTypeId).Name}'");

            if (!idToType!.TryGetValue(getVariantNameFromAncestor(parent), out var type))
                throw new SerializationException($"No implementation for '{getVariantNameFromAncestor(parent)}'");

            var typed = variantNode.ToObject(type, serializer);
            if (typed == null)
                throw new SerializationException($"Couldn't deserialize {type.Name}");

            return (parent, (TVariant)typed);
        }
    }

    public class PlatformConditionJsonConverter : CovariantConverter<LogAlertsV2AlertContext, IConditionPart[]> //JsonConverter
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

    public class AlertDataJsonConverter : CovariantConverter<Data, IAlertContext>
    {
        public override bool CanRead => true;
        public override Data? ReadJson(JsonReader reader, Type objectType, Data? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var variantName = new CamelCaseNamingStrategy().GetPropertyName(nameof(Data.AlertContext), false);
            var (parent, variant) = InstantiateCovariant(obj, variantName, ancestor => ancestor.Essentials.MonitoringService, variant => variant.MonitoringServices, serializer);

            parent.AlertContext = variant!;

            return parent;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, Data? value, JsonSerializer serializer) => throw new NotImplementedException();
    }

    //public class AlertContextJsonConverter : JsonConverter<IAlertContext>
    //{
    //    public override bool CanRead => true;
    //    public override bool CanWrite => true;

    //    public override IAlertContext? ReadJson(JsonReader reader, Type objectType, IAlertContext? existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, IAlertContext? value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
