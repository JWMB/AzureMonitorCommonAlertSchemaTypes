using Shouldly;
using Types;
using Types.AlertContexts;

namespace Tests
{
    public class ActionGroupTests
    {
        [Fact]
        public void Deserialize_ActivityLog()
        {
            var alert = GetDeserialized("Activity log alert.json");

            alert.Data.Essentials.SignalType.ShouldBe("Activity Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("Activity Log - Administrative");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is Types.AlertContexts.ActivityLogAlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.Authorization.Action.ShouldBe("Microsoft.Compute/virtualMachines/restart/action");

            typed.Properties.EventCategory.ShouldBe("Administrative");

            typed.EventTimestamp.ShouldBe(DateTimeOffset.Parse("2022-11-07T12:01:33.048Z"));
            typed.EventSource.ShouldBe("Administrative");
            typed.OperationName.ShouldBe("Microsoft.Compute/virtualMachines/restart/action");
        }

        [Fact]
        public void Deserialize_AvailabilityTest()
        {
            var alert = GetDeserialized("Availability test alert.json");

            alert.Data.Essentials.SignalType.ShouldBe("Metric");
            alert.Data.Essentials.MonitoringService.ShouldBe("Platform");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is LogAlertsV2AlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.ConditionType.ShouldBe("WebtestLocationAvailabilityCriteria");

            typed.Condition.AllOf.ShouldNotBeEmpty();

            if (typed.Condition.AllOf is not Types.AlertContexts.LogAlertsV2.WebtestLocationAvailabilityCriteria[] conditions)
                throw new Exception($"{nameof(typed.Condition.AllOf)} is {typed.Condition.AllOf.GetType().Name}");

            conditions.Single().MetricName.ShouldBe("Failed Location");
            conditions.Single().MetricValue.ShouldBe(5M);

            typed.Condition.ToUserFriendlyString().ShouldBe("Failed Location: 5 > 2 (14:50:54 UTC:+00:00)");
        }

        [Fact]
        public void Deserialize_MetricStatic()
        {
            var alert = GetDeserialized("Metric alert - Static threshold.json");

            alert.Data.Essentials.SignalType.ShouldBe("Metric");
            alert.Data.Essentials.MonitoringService.ShouldBe("Platform");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is LogAlertsV2AlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.ConditionType.ShouldBe("SingleResourceMultipleMetricCriteria");

            typed.Condition.AllOf.ShouldNotBeEmpty();

            if (typed.Condition.AllOf is not Types.AlertContexts.LogAlertsV2.SingleResourceMultipleMetricCriteria[] conditions)
                throw new Exception($"{nameof(typed.Condition.AllOf)} is {typed.Condition.AllOf.GetType().Name}");

            conditions.Single().MetricName.ShouldBe("Transactions");
            conditions.Single().Dimensions!.Single().Name.ShouldBe("ApiName");

            typed.Condition.ToUserFriendlyString().ShouldBe("Transactions: 100 > 0 (12:13:31 UTC:+00:00)");
        }

        [Fact]
        public void Deserialize_MetricDynamic()
        {
            var alert = GetDeserialized("Metric alert - Dynamic threshold.json");

            alert.Data.Essentials.SignalType.ShouldBe("Metric");
            alert.Data.Essentials.MonitoringService.ShouldBe("Platform");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is LogAlertsV2AlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.ConditionType.ShouldBe("DynamicThresholdCriteria");

            typed.Condition.AllOf.ShouldNotBeEmpty();

            if (typed.Condition.AllOf is not Types.AlertContexts.LogAlertsV2.DynamicThresholdCriteria[] conditions)
                throw new Exception($"{nameof(typed.Condition.AllOf)} is {typed.Condition.AllOf.GetType().Name}");

            conditions.Single().MetricName.ShouldBe("Transactions");
            conditions.Single().MetricValue.ShouldBe(78.09M);
            conditions.Single().FailingPeriods.NumberOfEvaluationPeriods.ShouldBe(3);

            typed.Condition.ToUserFriendlyString().ShouldBe("Transactions: 78.09 > 0.3 (14:05:40 UTC:+00:00)");
        }


        [Fact]
        public void Deserialize_Smart()
        {
            var alert = GetDeserialized("Smart alert.json");

            alert.Data.Essentials.SignalType.ShouldBe("Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("SmartDetector");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is SmartAlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.DetectedValue.ShouldBe("0.45 sec");
            typed.AnalysisTimestamp.ShouldBe(DateTimeOffset.Parse("2022-11-07T12:26:55.591Z"));
        }

        [Fact]
        public void Deserialize_ServiceHealth()
        {
            var alert = GetDeserialized("Service health alert.json");

            alert.Data.Essentials.SignalType.ShouldBe("Activity Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("ServiceHealth");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is ServiceHealthAlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.Properties.ImpactMitigationTime.ShouldBe(DateTimeOffset.Parse("2022-11-07T13:13:05.521Z"));
            typed.Properties.ImpactType.ShouldBe("SubscriptionList");
            typed.Properties.IsSynthetic.ShouldBe("True");
        }

        [Fact]
        public void Deserialize_ResourceHealth()
        {
            var alert = GetDeserialized("Resource health alert.json");

            alert.Data.Essentials.SignalType.ShouldBe("Activity Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("Resource Health");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is ResourceHealthAlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.Properties.Title.ShouldBe("Rebooted by user");
            typed.Properties.CurrentHealthStatus.ShouldBe("Unavailable");
            typed.SubmissionTimestamp.ShouldBe(DateTimeOffset.Parse("2022-11-07T14:33:10.068Z"));
        }

        [Fact]
        public void Deserialize_LogAlertV2()
        {
            var alert = GetDeserialized("Log alert V2.json");

            alert.Data.Essentials.SignalType.ShouldBe("Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("Log Alerts V2");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is LogAlertsV2AlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.ConditionType.ShouldBe("LogQueryCriteria");

            typed.Condition.AllOf.ShouldNotBeEmpty();
            typed.Condition.ToUserFriendlyString().ShouldBe("Heartbeat/MMC: 3 > 0 (16:21:24 UTC:+00:00)");

            if (typed.Condition.AllOf is not Types.AlertContexts.LogAlertsV2.LogQueryCriteria[] conditions)
                throw new Exception($"{nameof(typed.Condition.AllOf)} is {typed.Condition.AllOf.GetType().Name}");

            conditions.Single().SearchQuery.ShouldBe("Heartbeat");
            conditions.Single().LinkToFilteredSearchResultsUi.ShouldNotBeNull();
            conditions.Single().ToUserFriendlyString().ShouldBe("Heartbeat/MMC: 3 > 0");
        }

        [Fact]
        public void Deserialize_LogAlertV1Metric()
        {
            var alert = GetDeserialized("Log alert V1 - Metric.json");

            alert.Data.Essentials.SignalType.ShouldBe("Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("Log Analytics");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is LogAnalyticsAlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.SearchQuery.ShouldBe("Heartbeat | summarize AggregatedValue=count() by bin(TimeGenerated, 5m)");

            typed.SearchIntervalStartTimeUtc.ShouldBe(DateTimeOffset.Parse("2022-11-23T16:31:12.512Z"));
            typed.SearchIntervalEndtimeUtc.ShouldBe(DateTimeOffset.Parse("2022-11-23T16:31:12.512Z"));

            typed.LinkToFilteredSearchResultsUi.ShouldNotBeNull();

            typed.SearchResults.Tables.Single().Columns.Select(o => o.Name).ShouldBe(new[] { "TimeGenerated", "AggregatedValue" });
        }

        private Alert GetDeserialized(string filename)
        {
            return Helpers.DeserializeFile($"Json/ActionGroupTests/{filename}");
        }
    }
}
