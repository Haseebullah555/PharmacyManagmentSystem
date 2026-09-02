using System.Globalization;
namespace Application.Helper
{
    public static class PersainDateHelper
    {
        public static DateTime ConverToDateTime(string persainDateTime)
        {
            string normalized = NormalizePersianNumbers(persainDateTime);

            var parts = normalized.Split(' ');
            var dateParts = parts[0].Split('-');
            var timeParts = parts.Length > 1 ? parts[1].Split(':') : new[] { "0", "0", "0" };

            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);

            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            int second = timeParts.Length > 2 ? int.Parse(timeParts[2]) : 0;


            PersianCalendar pc = new PersianCalendar();

            var date = pc.ToDateTime(year, month, day, hour, minute, second, 0);

            return DateTime.SpecifyKind(date, DateTimeKind.Utc);


        }
        private static string NormalizePersianNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");

            return input
                .Replace("۰", "0").Replace("۱", "1").Replace("۲", "2")
                .Replace("۳", "3").Replace("۴", "4").Replace("۵", "5")
                .Replace("۶", "6").Replace("۷", "7").Replace("۸", "8")
                .Replace("۹", "9");
        }
        public static DateTime ConvertPersainDateTimeToGregorian(DateOnly date)
        {
            // Convert DateOnly to DateTime first (assume time 00:00:00)
            var dateTime = date.ToDateTime(TimeOnly.MinValue);

            // Now specify UTC kind
            var utcDate = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return utcDate;
        }
        public static DateOnly ConvertPersainDateToGregorian(DateOnly persianDate)
        {
            // Persian calendar instance
            PersianCalendar pc = new PersianCalendar();

            // Extract Persian year, month, and day
            int year = persianDate.Year;
            int month = persianDate.Month;
            int day = persianDate.Day;

            // Convert Persian date to Gregorian DateTime
            DateTime gregorianDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);

            // Return as DateOnly
            return DateOnly.FromDateTime(gregorianDate);
        }
        public static DateOnly ConvertToDateOnly(string persianDate)
        {
            string normalized = NormalizePersianNumbers(persianDate);
            var dateParts = normalized.Split('-');

            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);

            PersianCalendar pc = new PersianCalendar();
            var dateTime = pc.ToDateTime(year, month, day, 0, 0, 0, 0);

            return DateOnly.FromDateTime(dateTime);
        }

        // ✅ NEW: Convert DateOnly to Persian date string (yyyy/MM/dd)
        public static string ConvertToPersianDateString(DateOnly dateOnly)
        {
            var dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(dateTime);
            int month = pc.GetMonth(dateTime);
            int day = pc.GetDayOfMonth(dateTime);

            return $"{year:0000}-{month:00}-{day:00}";
        }
    }
}
