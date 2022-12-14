using Newtonsoft.Json;
using System;
using AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2;

namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/azure-monitor/alerts/alerts-common-schema-test-action-definitions#monitoringservice--log-alerts-v1--metric
    /// </summary>
    public class LogAnalyticsAlertContext : IAlertContext
    {
        public string[] MonitoringServiceMatches => new[] { "Log Analytics", "Application Insights" };

        public string OperatorToken => OperatorValues.Get(Operator);

        public string ToUserFriendlyString()
        {
            return $"{SearchQuery}: {ResultCount} {OperatorToken} {Threshold}";
        }

        // Using JsonProperty here, since camelCase is used in all other JSON structures

        [JsonProperty("SearchQuery")]
        public string SearchQuery { get; set; } = string.Empty;

        [JsonProperty("SearchIntervalStartTimeUtc")]
        public DateTimeOffset SearchIntervalStartTimeUtc { get; set; }

        [JsonProperty("SearchIntervalEndtimeUtc")]
        public DateTimeOffset SearchIntervalEndtimeUtc { get; set; }

        [JsonProperty("ResultCount")]
        public long ResultCount { get; set; }

        [JsonProperty("LinkToSearchResults")]
        public Uri? LinkToSearchResults { get; set; } // = new Uri("");

        [JsonProperty("LinkToFilteredSearchResultsUI")]
        public Uri? LinkToFilteredSearchResultsUi { get; set; }

        [JsonProperty("LinkToSearchResultsAPI")]
        public Uri? LinkToSearchResultsApi { get; set; }

        [JsonProperty("LinkToFilteredSearchResultsAPI")]
        public Uri? LinkToFilteredSearchResultsApi { get; set; }

        [JsonProperty("SeverityDescription")]
        public string SeverityDescription { get; set; } = string.Empty;

        [JsonProperty("WorkspaceId")]
        public string WorkspaceId { get; set; } = string.Empty;

        [JsonProperty("SearchIntervalDurationMin")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long SearchIntervalDurationMin { get; set; }

        [JsonProperty("AffectedConfigurationItems")]
        public string[]? AffectedConfigurationItems { get; set; }

        [JsonProperty("SearchIntervalInMinutes")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long SearchIntervalInMinutes { get; set; }

        [JsonProperty("Threshold")]
        public long Threshold { get; set; }

        [JsonProperty("Operator")]
        public string Operator { get; set; } = string.Empty; // TODO: Seems this one is using e.g. "Greater Than" rather then "GreaterThan"..?

        [JsonProperty("Dimensions")]
        public Dimension[]? Dimensions { get; set; }

        [JsonProperty("SearchResults")]
        public SearchResults SearchResults { get; set; } = new SearchResults();

        [JsonProperty("IncludedSearchResults")]
        public string IncludedSearchResults { get; set; } = string.Empty;

        [JsonProperty("AlertType")]
        public string AlertType { get; set; } = string.Empty;
    }

    public class Dimension
    {
        public string Name { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }

    public class SearchResults
    {
        public Table[] Tables { get; set; } = new Table[0];

        public DataSource[] DataSources { get; set; } = new DataSource[0];
    }

    public class DataSource
    {
        public string ResourceId { get; set; } = string.Empty;

        public string[] Tables { get; set; } = new string[0];
    }

    public class Table
    {
        public string Name { get; set; } = string.Empty;

        public Column[] Columns { get; set; } = new Column[0];

        public string[][] Rows { get; set; } = new string[0][];
    }

    public class Column
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}

