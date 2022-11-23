using System.Globalization;

namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2
{
    public abstract class ThresholdCriteriaBase : IConditionPart
    {
        public abstract string[] ConditionTypeMatch { get; }

        public virtual string ToUserFriendlyString() => $"{MetricName}: {MetricOperatorThresholdString()}";

        public string MetricOperatorThresholdString() => $"{MetricValue.ToString(CultureInfo.InvariantCulture)} {OperatorToken} {Threshold}";

        public string MetricName { get; set; } = string.Empty;
        public string MetricNamespace { get; set; } = string.Empty;

        public string Operator { get; set; } = string.Empty;

        public string Threshold { get; set; } = string.Empty;
        public string TimeAggregation { get; set; } = string.Empty;

        public Dimension[]? Dimensions { get; set; }

        public decimal MetricValue { get; set; }
        public string? WebTestName { get; set; }

        public string OperatorToken => OperatorValues.Get(Operator);
    }
}
