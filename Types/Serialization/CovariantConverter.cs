using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Types.Serialization
{
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
}
