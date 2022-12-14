using AzureMonitorCommonAlertSchemaTypes;
using Newtonsoft.Json;
using Shouldly;
using AzureMonitorCommonAlertSchemaTypes.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tests
{
    public class GenerateDocumentation
    {
        [Fact]
        public void ListContexts()
        {
            var baseType = typeof(IAlertContext);
            XmlDocTools.LoadXmlDocumentation(baseType.Assembly);

            var str = string.Join("\n", TraverseImplementations(baseType, ""));

            var filename = Helpers.ResolveFilename("../README.md");
            var readme = File.ReadAllText(filename);
            if (ReplaceSection(readme, "Variants", str, out var replaced))
                File.WriteAllText(filename, replaced);
        }

        private bool ReplaceSection(string content, string sectionId, string sectionContent, out string contentReplaced)
        {
            var pattern = $@"({SectionMarkerPattern("BEGIN", sectionId)})(.+)({SectionMarkerPattern("END", sectionId)})";
            var rx = new Regex(pattern, RegexOptions.Singleline);
            contentReplaced = rx.Replace(content, $"$1\n{sectionContent}\n$3");
            return contentReplaced != content;

            string SectionMarkerPattern(string marker, string sectionId) => $@"{Regex.Escape($"<!-- {marker}:{sectionId} -->")}";
        }

        private IEnumerable<string> TraverseImplementations(Type type, string prefix)
        {
            var types = TypeHelper.GetTypesDerivedFrom(type);
            foreach (var t in types)
            {
                var result = $"{prefix}* {t.Name}";
                {
                    var tmp = XmlDocTools.GetInnerText(XmlDocTools.GetDocumentation(t));
                    if (!string.IsNullOrEmpty(tmp))
                        result = $"{result} - [example]({tmp})";
                }
                yield return result;

                var interfaceProps = GetInterfaceTypeProperties(t);
                if (interfaceProps.Any())
                {
                    foreach (var sub in interfaceProps)
                    {
                        foreach (var yielded in TraverseImplementations(sub.PropertyType, $"{prefix}\t"))
                            yield return yielded;
                    }
                }
            }
        }

        private IEnumerable<PropertyInfo> GetInterfaceTypeProperties(Type type)
        {
            var writable = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && IsBuiltIn(p.PropertyType) == false)
                .ToList();

            foreach (var item in writable.Where(p => IsInterfacey(p.PropertyType)))
                yield return item;

            foreach (var item in writable)
                foreach (var yielded in GetInterfaceTypeProperties(item.PropertyType))
                    yield return yielded;

            bool IsInterfacey(Type t)
            {
                if (t.IsInterface)
                    return true;

                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(t))
                {
                    var elType = t.GetElementType();
                    if (elType != null)
                        return IsInterfacey(elType);
                }

                if (t.IsGenericType)
                    return t.GenericTypeArguments.Any(x => IsInterfacey(x));
                
                return false;
            }
            
            bool IsBuiltIn(Type t)
            {
                if (t.IsPrimitive || t == typeof(string) || t == typeof(DateTimeOffset) || t == typeof(DateTime))
                    return true;

                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(t))
                {
                    if (t.IsGenericType)
                        return t.GenericTypeArguments.All(x => IsBuiltIn(x));
                }

                return false;
            }
        }
    }
}