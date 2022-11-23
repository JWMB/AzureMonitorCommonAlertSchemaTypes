using System.Globalization;

namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#metric-alerts---dynamic-threshold
    /// </summary>
    public class DynamicThresholdCriteria : ThresholdCriteriaBase
    {
        public override string[] ConditionTypeMatch => new[] { "DynamicThresholdCriteria" };

        public string AlertSensitivity { get; set; } = string.Empty;
        public FailingPeriods FailingPeriods { get; set; } = new FailingPeriods();
    }
}
