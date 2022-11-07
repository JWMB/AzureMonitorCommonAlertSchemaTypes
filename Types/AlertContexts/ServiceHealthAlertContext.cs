using System;

namespace Types.AlertContexts
{
    public class ServiceHealthAlertContext : HealthContextBase
    {
        public override string[] MonitoringServices => new[] { "ServiceHealth" };
        public override string ToUserFriendlyString()
        {
            return $"{OperationName} {Status} {Properties.Title} {Properties.Service} {Properties.Region} {Properties.DefaultLanguageContent}"; // {EventSource}: {OperationName} {EventTimestamp:HH:mm:ss}";
        }

        // "authorization": null,
        public long Channels { get; set; }

        //"claims": null,
        //"caller": null,

        //public string CorrelationId { get; set; } = string.Empty; // WHAT, not a GUID? MS example is "12345678-abcd-efgh-ijkl-abcd12345678"

        public long EventSource { get; set; }
        //public DateTimeOffset EventTimestamp { get; set; }

        // "httpRequest": null,
        //public Guid EventDataId { get; set; }
        public long Level { get; set; }

        //public string OperationName { get; set; } = string.Empty;
        //public string OperationId { get; set; } = string.Empty; // WHAT, not a GUID? MS example is "12345678-abcd-efgh-ijkl-abcd12345678"

        public ContextProperties Properties { get; set; } = new ContextProperties();

        //public string Status { get; set; } = string.Empty;

        //"subStatus": null,

        //public DateTimeOffset SubmissionTimestamp { get; set; }

        //"ResourceType": null

        public class ContextProperties
        {
            public string Title { get; set; } = string.Empty;
            public string Service { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;

            public string Communication { get; set; } = string.Empty;
            public string IncidentType { get; set; } = string.Empty;
            public string TrackingId { get; set; } = string.Empty;
            public DateTimeOffset ImpactStartTime { get; set; }
            public DateTimeOffset ImpactMitigationTime { get; set; }
            public string ImpactedServicesTableRows { get; set; } = string.Empty;
            public string DefaultLanguageTitle { get; set; } = string.Empty;
            public string DefaultLanguageContent { get; set; } = string.Empty;
            public string Stage { get; set; } = string.Empty;
            public string CommunicationId { get; set; } = string.Empty;
            public string IsHIR { get; set; } = string.Empty;
            public string IsSynthetic { get; set; } = string.Empty;
            public string ImpactType { get; set; } = string.Empty;
            public string Version { get; set; } = string.Empty;

            // TODO:
            //"impactedServices": [
            //  {
            //    "ImpactedRegions": [ { "RegionName": "Global" } ],
            //    "ServiceName": "Azure Service Name"
            //  }
            //],
        }
    }
}
