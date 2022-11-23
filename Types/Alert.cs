namespace AzureMonitorCommonAlertSchemaTypes
{
    // TODO: maybe we'd also like some structure like
    //public class Alert<T>
    //    where T : new()
    //  public Data<T> Data { get; set; } = new Data<T>();

    //public class Data<T>
    //    where T : new()
    //  public T AlertContext { get; set; } = new T();

    /// <summary>
    /// Root type for all alerts
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// The only supported value is currently 'azureMonitorCommonAlertSchema'
        /// </summary>
        public string SchemaId { get; set; } = string.Empty;

        /// <summary>
        /// All information except SchemaId
        /// </summary>
        public Data Data { get; set; } = new Data();
    }
}
