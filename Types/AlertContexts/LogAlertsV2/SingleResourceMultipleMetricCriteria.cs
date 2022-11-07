using System.Globalization;

namespace Types.AlertContexts.LogAlertsV2
{
    public class SingleResourceMultipleMetricCriteria : ThresholdCriteriaBase
    {
        public override string[] ConditionTypeMatch => new[] { "SingleResourceMultipleMetricCriteria" };

    }
}
