namespace Types.AlertContexts.LogAlertsV2
{
    public static class OperatorValues
    {
        public static string Get(string Operator)
        {
            switch (Operator)
            {
                case "GreaterThan":
                    return ">";
                case "GreaterThanOrEqual":
                    return ">=";
                //case "LessThan":
                case "LowerThan":
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
