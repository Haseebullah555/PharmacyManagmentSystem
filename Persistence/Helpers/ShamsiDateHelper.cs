using System.Globalization;

namespace Persistence.Helpers
{
    public static class ShamsiDateHelper
    {
        private static readonly PersianCalendar _pc = new PersianCalendar();

        public static int GetShamsiYear(DateTime date)
            => _pc.GetYear(date);

        public static int GetShamsiMonth(DateTime date)
            => _pc.GetMonth(date);
    }
}