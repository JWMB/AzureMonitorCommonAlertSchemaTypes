namespace Types
{
    public interface IAlertContext
    {
        string[] MonitoringServiceMatches { get; }
        string ToUserFriendlyString();
    }
}
