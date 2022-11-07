using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Types.AlertContexts.LogAlertsV2;

namespace Types.AlertContexts
{
    public class LogAlertsV2AlertContext : IAlertContext
    {
        public string[] MonitoringServiceMatches => new[] { "Log Alerts V2", "Platform" };


        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public string ConditionType { get; set; } = string.Empty;

        public Condition Condition { get; set; } = new Condition();

        public string ToUserFriendlyString()
        {
            return Condition.ToUserFriendlyString();
        }

    }
    public class Condition
    {
        public string WindowSize { get; set; } = string.Empty;

        public IConditionPart[]? AllOf { get; set; }

        public DateTimeOffset WindowStartTime { get; set; }

        public DateTimeOffset WindowEndTime { get; set; }

        public string ToUserFriendlyString()
        {
            return $"{string.Join(", ", AllOf.Select(o => o.ToUserFriendlyString()))} ({GetUserFriendlyTimeWindowString()})";
        }

        public string GetUserFriendlyTimeWindowString()
        {
            var diff = WindowEndTime - WindowStartTime;

            string str;
            if (diff.TotalSeconds <= 1)
                str = DateToString(WindowStartTime, false);
            else
            {
                var isMoreThanADay = diff.TotalHours >= 24;
                str = $"{DateToString(WindowStartTime, isMoreThanADay)} - {DateToString(WindowEndTime, isMoreThanADay)}";
            }

            return $"{str} UTC:{WindowEndTime:zzz}";

            string DateToString(DateTimeOffset date, bool includeDate) =>
    date.ToString(includeDate ? "dd/MM HH:mm:ss" : "HH:mm:ss", CultureInfo.InvariantCulture);

        }
    }
}
