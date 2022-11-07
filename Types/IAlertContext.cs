namespace Types
{
    public interface IAlertContext
    {
        string[] MonitoringServices { get; }
        string ToUserFriendlyString();
    }
}
