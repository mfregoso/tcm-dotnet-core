using System;
using TCM.Models.Domain;

namespace TCM.Services.Utils
{
    public class DateHelpers
    {
        public static DateTime GetTmiExpiration()
        {
            var nextWeek = DateTime.UtcNow.AddDays(7);
            return nextWeek;
        }

        public static DateTime GetHistoryExpiration()
        {
            var nextMo = DateTime.UtcNow.AddMonths(1);
            return new DateTime(nextMo.Year, nextMo.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public static bool IsExpired(DateTime? expirationDate)
        {
            return DateTime.UtcNow > expirationDate ? true : false;
        }
    }
}
