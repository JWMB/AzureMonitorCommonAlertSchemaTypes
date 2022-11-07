using System.Globalization;

namespace Types.AlertContexts.LogAlertsV2
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#metric-alerts---static-threshold
    /// </summary>
    public class SingleResourceMultipleMetricCriteria : ThresholdCriteriaBase
    {
        public override string[] ConditionTypeMatch => new[] { "SingleResourceMultipleMetricCriteria" };

    }
}
