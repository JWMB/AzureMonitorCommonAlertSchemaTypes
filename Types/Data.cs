using System.Collections.Generic;

namespace AzureMonitorCommonAlertSchemaTypes
{
    public class Data
    {
        public Essentials Essentials { get; set; } = new Essentials();
        public IAlertContext? AlertContext { get; set; }
        public Dictionary<string, string>? CustomProperties { get; set; }
    }
}
