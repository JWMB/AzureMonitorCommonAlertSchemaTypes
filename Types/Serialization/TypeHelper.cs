using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureMonitorCommonAlertSchemaTypes.Serialization
{
    public class TypeHelper
    {
        public static IEnumerable<Type> GetTypesDerivedFrom(Type baseType)
        {
            var isArray = baseType.IsArray;
            if (isArray)
            {
                baseType = baseType.GetElementType();
            }
            return baseType.Assembly.GetExportedTypes()
                .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract);
        }
    }
}
