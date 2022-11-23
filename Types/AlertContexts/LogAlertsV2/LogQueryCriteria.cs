using System;

namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#monitoringservice--log-alerts-v2
    /// </summary>
    public class LogQueryCriteria : IConditionPart
    {
        public string[] ConditionTypeMatch => new[] { "LogQueryCriteria" };

        public string SearchQuery { get; set; } = string.Empty;

        public string MetricMeasureColumn { get; set; } = string.Empty;

        public string TargetResourceTypes { get; set; } = string.Empty;

        public string Operator { get; set; } = string.Empty;
        
        public long Threshold { get; set; }

        public string TimeAggregation { get; set; } = string.Empty;

        public Dimension[]? Dimensions { get; set; }

        public long MetricValue { get; set; }

        public FailingPeriods FailingPeriods { get; set; } = new FailingPeriods();

        public Uri? LinkToSearchResultsUi { get; set; }

        public Uri? LinkToFilteredSearchResultsUi { get; set; }

        public Uri? LinkToSearchResultsApi { get; set; }

        public Uri? LinkToFilteredSearchResultsApi { get; set; }

        public string OperatorToken => OperatorValues.Get(Operator);
        public string ToUserFriendlyString()
        {
            return $"{SearchQuery.NullOrEmptyDefault("SQ")}/{MetricMeasureColumn.NullOrEmptyDefault("MMC")}: {MetricValue} {OperatorToken} {Threshold}";
        }
    }
}
