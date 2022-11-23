namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2
{
    public interface IConditionPart
    {
        string[] ConditionTypeMatch { get; }
        string ToUserFriendlyString();
    }
}
