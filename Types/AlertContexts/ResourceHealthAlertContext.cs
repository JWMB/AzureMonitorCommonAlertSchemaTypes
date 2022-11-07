namespace Types.AlertContexts
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#monitoringservice--resource-health
    /// </summary>
    public class ResourceHealthAlertContext : HealthContextBase
    {
        public override string[] MonitoringServiceMatches => new[] { "Resource Health" };
        public override string ToUserFriendlyString()
        {
            return $"{OperationName} {Status} {Properties.Title} {Properties.Details} {Properties.Cause}";
        }

        //"Activity Log Event Description": null

        public string Channels { get; set; } = string.Empty;

        public string EventSource { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;


        public ContextProperties Properties { get; set; } = new ContextProperties();


        public class ContextProperties
        {
            public string Title { get; set; } = string.Empty;
            public string Details { get; set; } = string.Empty;
            public string CurrentHealthStatus { get; set; } = string.Empty;
            public string PreviousHealthStatus { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Cause { get; set; } = string.Empty;
        }
    }
}
