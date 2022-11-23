namespace AzureMonitorCommonAlertSchemaTypes.AlertContexts.LogAlertsV2
{
    public static class OperatorValues
    {
        public static string Get(string Operator)
        {
            switch (Operator)
            {
                case "GreaterThan":
                case "Greater Than":
                    return ">";
                case "GreaterThanOrEqual":
                    return ">=";
                //case "LessThan":
                case "LowerThan":
                case "Lower Than":
                    return "<";
                //case "LessThanOrEqual":
                case "LowerThanOrEqual":
                    return "<=";
                case "Equal":
                    return "==";
                default:
                    return Operator;
            }
        }
    }
}
