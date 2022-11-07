using System;

namespace Types.AlertContexts
{
    public abstract class HealthContextBase : IAlertContext
    {
        public abstract string[] MonitoringServices { get; }

        public abstract string ToUserFriendlyString();

        public string CorrelationId { get; set; } = string.Empty; // WHAT, not a GUID? MS example is "12345678-abcd-efgh-ijkl-abcd12345678"
        public DateTimeOffset EventTimestamp { get; set; }
        public Guid EventDataId { get; set; }
        public string OperationName { get; set; } = string.Empty;
        public string OperationId { get; set; } = string.Empty; // WHAT, not a GUID? MS example is "12345678-abcd-efgh-ijkl-abcd12345678"
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset SubmissionTimestamp { get; set; }
    }
}
