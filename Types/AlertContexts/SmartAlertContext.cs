using System;

namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#monitoringservice--smart-alert
    /// </summary>
    public class SmartAlertContext : IAlertContext
    {
        public string[] MonitoringServiceMatches => new[] { "SmartDetector" };

        public string ToUserFriendlyString()
        {
            return $"{SmartDetectorName} - Detected:{DetectedValue} Normal:{NormalValue}";
        }

        public string DetectionSummary { get; set; } = string.Empty;

        public DateTimeOffset FormattedOccurrenceTime { get; set; }

        public string DetectedValue { get; set; } = string.Empty;

        public string NormalValue { get; set; } = string.Empty;

        public string PresentationInsightEventRequest { get; set; } = string.Empty;

        public string SmartDetectorId { get; set; } = string.Empty;

        public string SmartDetectorName { get; set; } = string.Empty;

        public DateTimeOffset AnalysisTimestamp { get; set; }
    }
}
