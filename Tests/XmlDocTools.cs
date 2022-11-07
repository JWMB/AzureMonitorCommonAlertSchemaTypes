using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Tests
{
    // based on https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/october/csharp-accessing-xml-documentation-via-reflection
    internal static class XmlDocTools
    {
        internal static Dictionary<string, string> loadedXmlDocumentation = new Dictionary<string, string>();

        public static void LoadXmlDocumentation(string xmlDocumentation)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(xmlDocumentation)))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
                    {
                        string? raw_name = xmlReader["name"];
                        if (raw_name != null)
                            loadedXmlDocumentation[raw_name] = xmlReader.ReadInnerXml();
                    }
                }
            }
        }

        public static string? GetDocumentation(this MemberInfo memberInfo)
        {
            if (memberInfo.MemberType.HasFlag(MemberTypes.Field))
            {
                return ((FieldInfo)memberInfo).GetDocumentation();
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Property) && memberInfo is PropertyInfo pi)
            {
                return pi.GetDocumentation();
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Event))
            {
                return ((EventInfo)memberInfo).GetDocumentation();
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Constructor))
            {
                return ((ConstructorInfo)memberInfo).GetDocumentation();
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Method))
            {
                return ((MethodInfo)memberInfo).GetDocumentation();
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.TypeInfo) ||
              memberInfo.MemberType.HasFlag(MemberTypes.NestedType))
            {
                return ((TypeInfo)memberInfo).GetDocumentation();
            }
            else
            {
                return null;
            }
        }

        public static string GetDocumentation(this ParameterInfo parameterInfo)
        {
            var memberDocumentation = parameterInfo.Member.GetDocumentation();
            if (memberDocumentation != null)
            {
                var regexPattern =
                  Regex.Escape(@"<param name=" + "\"" + parameterInfo.Name + "\"" + @">") +
                  ".*?" +
                  Regex.Escape(@"</param>");
                var match = Regex.Match(memberDocumentation, regexPattern);
                if (match.Success)
                {
                    return match.Value;
                }
            }
            return null;
        }

        public static string GetDirectoryPath(this Assembly assembly)
        {
            var path = assembly.Location;
            //var codeBase = assembly.CodeBase;
            //var uri = new UriBuilder(codeBase);
            //var path = Uri.UnescapeDataString(uri.Path);
            //if (path == null)
            //    throw new Exception("");
            return Path.GetDirectoryName(path);
        }

        internal static HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();
        internal static void LoadXmlDocumentation(Assembly assembly)
        {
            if (loadedAssemblies.Contains(assembly))
            {
                return; // Already loaded
            }
            var directoryPath = assembly.GetDirectoryPath();
            var xmlFilePath = Path.Combine(directoryPath, assembly.GetName().Name + ".xml");
            if (File.Exists(xmlFilePath))
            {
                LoadXmlDocumentation(File.ReadAllText(xmlFilePath));
                loadedAssemblies.Add(assembly);
            }
        }

        // Helper method to format the key strings
        private static string XmlDocumentationKeyHelper(string typeFullNameString, string? memberNameString)
        {
            var key = Regex.Replace(
              typeFullNameString, @"\[.*\]",
              string.Empty).Replace('+', '.');
            if (memberNameString != null)
            {
                key += "." + memberNameString;
            }
            return key;
        }
        public static string? GetDocumentation(this Type type)
        {
            if (type?.FullName == null)
                return null;
            var key = "T:" + XmlDocumentationKeyHelper(type.FullName, null);
            loadedXmlDocumentation.TryGetValue(key, out string? documentation);
            return documentation;
        }

        public static string? GetDocumentation(this PropertyInfo propertyInfo)
        {
            if (propertyInfo?.DeclaringType?.FullName == null)
                return null;
            var key = "P:" + XmlDocumentationKeyHelper(
              propertyInfo.DeclaringType.FullName, propertyInfo.Name);
            loadedXmlDocumentation.TryGetValue(key, out string? documentation);
            return documentation;
        }

        public static string? GetInnerText(string? comment)
        {
            return comment == null ? comment : string.Join("\n", comment.Split('\n').Select(o => o.Trim()).Where(o => o.Length > 0 && !o.StartsWith("<")));
        }
    }
}
