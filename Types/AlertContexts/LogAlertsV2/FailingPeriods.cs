namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2
{
    public class FailingPeriods
    {
        public long NumberOfEvaluationPeriods { get; set; }

        public long MinFailingPeriodsToAlert { get; set; }
    }
}
