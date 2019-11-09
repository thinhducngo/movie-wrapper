using System;
using System.Globalization;

namespace MovieWrapper.Utils
{
    /// <summary>
    /// Format data to specify type
    /// </summary>
    public static class Formatter
    {
        public static DateTime? FormatToDateTime(string value, string format)
        {
            if (string.IsNullOrEmpty(value)) return null;
            else return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }
    }
}
