namespace AzureMonitorCommonAlertSchemaTypes
{
    public interface IAlertContext
    {
        string[] MonitoringServiceMatches { get; }
        string ToUserFriendlyString();
    }
}
