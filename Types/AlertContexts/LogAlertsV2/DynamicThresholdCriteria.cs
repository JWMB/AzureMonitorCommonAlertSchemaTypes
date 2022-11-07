using System.Globalization;

namespace Types.AlertContexts.LogAlertsV2
{

    public class DynamicThresholdCriteria : ThresholdCriteriaBase
    {
        public override string[] ConditionTypeMatch => new[] { "DynamicThresholdCriteria" };

        public string AlertSensitivity { get; set; } = string.Empty;
        public FailingPeriods FailingPeriods { get; set; } = new FailingPeriods();
    }
}
