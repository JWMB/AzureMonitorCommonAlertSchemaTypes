using System;

namespace AzureMonitorCommonAlertSchemaTypes
{
    public class Essentials
    {
        public string AlertId { get; set; } = string.Empty;
        public string AlertRule { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string SignalType { get; set; } = string.Empty;
        public string MonitorCondition { get; set; } = string.Empty;
        public string MonitoringService { get; set; } = string.Empty;
        public string[] AlertTargetIDs { get; set; } = new string[0];
        public string[] ConfigurationItems { get; set; } = new string[0];
        public string OriginAlertId { get; set; } = string.Empty;
        public DateTimeOffset? FiredDateTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EssentialsVersion { get; set; } = string.Empty;
        public string AlertContextVersion { get; set; } = string.Empty;
    }
}
