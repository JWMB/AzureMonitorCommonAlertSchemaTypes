using System;

namespace Types.AlertContexts
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#monitoringservice--activity-log---administrative
    /// </summary>
    public class ActivityLogAlertContext : IAlertContext
    {
        public string[] MonitoringServiceMatches => new[] { "Activity Log", "Activity Log - Administrative" };
        public string ToUserFriendlyString()
        {
            return $"{EventSource}: {OperationName} {EventTimestamp:HH:mm:ss}";
        }

        public Authorization Authorization { get; set; } = new Authorization();

        public string Channels { get; set; } = string.Empty;

        public string Claims { get; set; } = string.Empty;

        public string Caller { get; set; } = string.Empty;

        public Guid CorrelationId { get; set; }

        public string EventSource { get; set; } = string.Empty;

        public DateTimeOffset EventTimestamp { get; set; }

        public Guid EventDataId { get; set; }

        public string Level { get; set; } = string.Empty;

        public string OperationName { get; set; } = string.Empty;

        public Guid OperationId { get; set; }

        public Properties Properties { get; set; } = new Properties(); // TODO: should this be Dictionary<string, string>?

        public string Status { get; set; } = string.Empty;

        public string SubStatus { get; set; } = string.Empty;

        public DateTimeOffset SubmissionTimestamp { get; set; }

        public string ActivityLogEventDescription { get; set; } = string.Empty;
    }

        public class Authorization
        {
            public string Action { get; set; } = string.Empty;

            public string Scope { get; set; } = string.Empty;
        }

        public class Properties
        {
            public string EventCategory { get; set; } = string.Empty;

            public string Entity { get; set; } = string.Empty;

            public string Message { get; set; } = string.Empty;

            public string Hierarchy { get; set; } = string.Empty;
    }
}

