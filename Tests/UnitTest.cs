using Types;
using Newtonsoft.Json;
using Shouldly;
using Types.AlertContexts;
using Types.Serialization;

namespace Tests
{
    public class UnitTest
    {
        // https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-definitions
        [Fact]
        public void Deserialize_LogAnalytics()
        {
            var alert = Helpers.DeserializeFile("Json/logSearchAlerts.json");

            alert.SchemaId.ShouldBe("azureMonitorCommonAlertSchema");

            alert.Data.Essentials.SignalType.ShouldBe("Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("Log Analytics");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (alert.Data.AlertContext is not LogAnalyticsAlertContext typed)
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.AffectedConfigurationItems.ShouldBe(new[] { "FS01.Sandlot.dom" });
            typed.SearchResults.Tables.Single().Name.ShouldBe("PrimaryResult");
        }

        [Fact]
        public void Deserialize_LogAlertsV2()
        {
            var alert = Helpers.DeserializeFile("Json/platform.json");

            alert.Data.Essentials.SignalType.ShouldBe("Log");
            alert.Data.Essentials.MonitoringService.ShouldBe("Platform");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (!(alert.Data.AlertContext is LogAlertsV2AlertContext typed))
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.ConditionType.ShouldBe("LogQueryCriteria");

            typed.Condition.AllOf.ShouldNotBeEmpty();

            if (typed.Condition.AllOf is not Types.AlertContexts.LogAlertsV2.LogQueryCriteria[] conditions)
                throw new Exception($"{nameof(typed.Condition.AllOf)} is {typed.Condition.AllOf.GetType().Name}");

            conditions.Single().SearchQuery.ShouldBe("Heartbeat");
            conditions.Single().Dimensions!.Single().Name.ShouldBe("Computer");
            conditions.Single().MetricMeasureColumn.ShouldBe("CounterValue");
            
            typed.Condition.ToUserFriendlyString().ShouldBe("Heartbeat/CounterValue: 0 < 1 (07/07 13:54:34 - 09/07 13:54:34 UTC:+00:00)");
        }

        [Fact]
        public void Deserialize_ApplicationInsights()
        {
            var alert = DeserializeAlertContextFile("Json/Application Insights - context.json", "Application Insights");

            alert.Data.Essentials.MonitoringService.ShouldBe("Application Insights");

            alert.Data.AlertContext.ShouldNotBeNull();
            if (alert.Data.AlertContext is not LogAnalyticsAlertContext typed)
                throw new Exception($"Wrong type: {alert.Data.AlertContext.GetType().Name}");

            typed.AffectedConfigurationItems.ShouldBeNull();
            typed.SearchResults.Tables.Single().Rows.First().First().ShouldBe("Fabrikam");
        }

        private Alert DeserializeAlertContextFile(string filename, string monitoringService)
        {
            var str = File.ReadAllText(Helpers.ResolveFilename(filename));
            str = WrapAlertContext(str, monitoringService);
            return AlertJsonSerializerSettings.DeserializeOrThrow(str);
        }

        private string WrapAlertContext(string alertContext, string monitoringService)
        {
            return $@"{{
  ""schemaId"": ""azureMonitorCommonAlertSchema"",
  ""data"": {{
    ""essentials"": {{
      ""monitoringService"": ""{monitoringService}"",
    }},
    {alertContext.Trim().TrimStart('{').TrimEnd('}')}
  }}
}}";
        }
    }
}