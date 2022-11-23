using Shouldly;
using AzureMonitorCommonAlertSchemaTypes;
using AzureMonitorCommonAlertSchemaTypes.AlertContexts;
using AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2;

namespace Tests
{
    public class DemuxerTests
    {
        [Theory]
        [InlineData("Activity log alert", "ActivityLogAlertContext")]
        [InlineData("Availability test alert", "LogAlertsV2AlertContext/WebtestLocationAvailabilityCriteria")]
        [InlineData("Log alert V1 - Metric", "LogAnalyticsAlertContext")]
        [InlineData("Log alert V2", "LogAlertsV2AlertContext/LogQueryCriteria")]
        [InlineData("Metric alert - Dynamic threshold", "LogAlertsV2AlertContext/DynamicThresholdCriteria")]
        [InlineData("Metric alert - Static threshold", "LogAlertsV2AlertContext/SingleResourceMultipleMetricCriteria")]
        [InlineData("Resource health alert", "ResourceHealthAlertContext")]
        [InlineData("Service health alert", "ServiceHealthAlertContext")]
        [InlineData("Smart alert", "SmartAlertContext")]
        public void AlertType_CorrectlyHandled(string file, string expected)
        {
            var alert = GetDeserialized($"{file}.json");
            var demuxedLog = new DemuxedLog();
            var demuxer = new AlertDemuxer(demuxedLog);
            demuxer.Demux(alert);

            demuxedLog.Received.Single().ShouldBe(expected);
        }

        [Fact]
        public void AlertType_AllExistingJsonFilesAreHandled()
        {
            var folder = new FileInfo(Helpers.ResolveFilename("Json/ActionGroupTests/Activity log alert.json")).Directory;
            var files = folder.GetFiles("*.json");

            var demuxedLog = new DemuxedLog();
            var demuxer = new AlertDemuxer(demuxedLog);

            foreach (var file in files)
            {
                var alert = GetDeserialized(file.Name);
                demuxer.Demux(alert);
            }

            demuxedLog.Received.Count.ShouldBe(files.Length);
        }

        private Alert GetDeserialized(string filename)
        {
            return Helpers.DeserializeFile($"Json/ActionGroupTests/{filename}");
        }

        // TODO: use AutoFixture instead
        private class DemuxedLog : IDemuxedAlert
        {
            public List<string> Received = new List<string>();

            public void ActivityLogAlertContext(Alert alert, ActivityLogAlertContext ctx) => Received.Add("ActivityLogAlertContext");
            public void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx) => Received.Add("LogAlertsV2AlertContext");
            public void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, DynamicThresholdCriteria criteria) => Received.Add("LogAlertsV2AlertContext/DynamicThresholdCriteria");
            public void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, LogQueryCriteria criteria) => Received.Add("LogAlertsV2AlertContext/LogQueryCriteria");
            public void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, SingleResourceMultipleMetricCriteria criteria) => Received.Add("LogAlertsV2AlertContext/SingleResourceMultipleMetricCriteria");
            public void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, WebtestLocationAvailabilityCriteria criteria) => Received.Add("LogAlertsV2AlertContext/WebtestLocationAvailabilityCriteria");
            public void LogAnalyticsAlertContext(Alert alert, LogAnalyticsAlertContext ctx) => Received.Add("LogAnalyticsAlertContext");
            public void ResourceHealthAlertContext(Alert alert, ResourceHealthAlertContext ctx) => Received.Add("ResourceHealthAlertContext");
            public void ServiceHealthAlertContext(Alert alert, ServiceHealthAlertContext ctx) => Received.Add("ServiceHealthAlertContext");
            public void SmartAlertContext(Alert alert, SmartAlertContext ctx) => Received.Add("SmartAlertContext");
        }
    }
}
