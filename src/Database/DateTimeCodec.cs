using System;
using System.Globalization;

namespace RegionExtension.Database
{
    internal static class DateTimeCodec
    {
        public static string FormatUtc(DateTime value) =>
            value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);

        public static DateTime Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DateTime.UtcNow;

            if (DateTime.TryParseExact(value, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var exact))
                return exact.ToUniversalTime();

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var invariantParsed))
                return invariantParsed;

            if (DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var localParsed))
                return localParsed;

            return DateTime.UtcNow;
        }
    }
}
