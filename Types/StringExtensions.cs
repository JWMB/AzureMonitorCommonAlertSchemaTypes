namespace Types
{
    public static class StringExtensions
    {
        public static string NullOrEmptyDefault(this string? value, string defaultValue) => string.IsNullOrEmpty(value) ? defaultValue : value!;
    }
}
