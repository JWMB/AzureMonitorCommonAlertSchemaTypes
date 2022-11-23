using System;
using System.Linq;
using AzureMonitorCommonAlertSchemaTypes.AlertContexts;
using AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2;

namespace AzureMonitorCommonAlertSchemaTypes
{
    public class AlertDemuxer
    {
        private readonly IDemuxedAlert demuxed;

        public AlertDemuxer(IDemuxedAlert demuxed)
        {
            this.demuxed = demuxed;
        }

        public void Demux(Alert alert)
        {
            var context = alert.Data.AlertContext;
            if (context is ActivityLogAlertContext ala)
                demuxed.ActivityLogAlertContext(alert, ala);
            else if (context is LogAnalyticsAlertContext la)
                demuxed.LogAnalyticsAlertContext(alert, la);
            else if (context is ResourceHealthAlertContext rh)
                demuxed.ResourceHealthAlertContext(alert, rh);
            else if (context is ServiceHealthAlertContext sh)
                demuxed.ServiceHealthAlertContext(alert, sh);
            else if (context is SmartAlertContext sa)
                demuxed.SmartAlertContext(alert, sa);
            else if (context is LogAlertsV2AlertContext lav2)
            {
                var one = lav2.Condition.AllOf.FirstOrDefault();
                if (one == null)
                    demuxed.LogAlertsV2AlertContext(alert, lav2);
                else if (one is DynamicThresholdCriteria dt)
                    demuxed.LogAlertsV2AlertContext(alert, lav2, dt);
                else if (one is LogQueryCriteria lq)
                    demuxed.LogAlertsV2AlertContext(alert, lav2, lq);
                else if (one is SingleResourceMultipleMetricCriteria sr)
                    demuxed.LogAlertsV2AlertContext(alert, lav2, sr);
                else if (one is WebtestLocationAvailabilityCriteria wl)
                    demuxed.LogAlertsV2AlertContext(alert, lav2, wl);
                else
                    throw new NotImplementedException($"{one.GetType().Name}");
            }
            else if (context == null)
            { }
            else
                throw new NotImplementedException($"{context.GetType().Name}");
            // Doesn't work in C# 8...
            //Action? action = alert.Data.AlertContext switch
            //{
            //    ActivityLogAlertContext ctx => () => demuxed.ActivityLogAlertContext(alert, ctx),
            //    LogAlertsV2AlertContext ctx and ctx.Condition.AllOf.FirstOrDefault() is DynamicThresholdCriteria => () => demuxed.LogAlertsV2AlertContext(alert, ctx),
            //    LogAnalyticsAlertContext ctx => () => demuxed.LogAnalyticsAlertContext(alert, ctx),
            //    ResourceHealthAlertContext ctx => () => demuxed.ResourceHealthAlertContext(alert, ctx),
            //    ServiceHealthAlertContext ctx => () => demuxed.ServiceHealthAlertContext(alert, ctx),
            //    SmartAlertContext ctx => () => demuxed.SmartAlertContext(alert, ctx),
            //    _ => null
            //};
            //action?.Invoke();
        }
    }

    public interface IDemuxedAlert
    {
        void ActivityLogAlertContext(Alert alert, ActivityLogAlertContext ctx);
        void LogAnalyticsAlertContext(Alert alert, LogAnalyticsAlertContext ctx);
        void ResourceHealthAlertContext(Alert alert, ResourceHealthAlertContext ctx);
        void ServiceHealthAlertContext(Alert alert, ServiceHealthAlertContext ctx);
        void SmartAlertContext(Alert alert, SmartAlertContext ctx);

        void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx);
        void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, DynamicThresholdCriteria criteria);
        void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, LogQueryCriteria criteria);
        void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, SingleResourceMultipleMetricCriteria criteria);
        void LogAlertsV2AlertContext(Alert alert, LogAlertsV2AlertContext ctx, WebtestLocationAvailabilityCriteria criteria);
    }

}
