using System;
using TCM.Models.Domain;

namespace TCM.Services.Utils
{
    public class DateHelpers
    {
        private static readonly int EXP_HOUR_UTC = 18;

        public static DateTime SetExpiration(DateTime dT)
        {
            return new DateTime(dT.Year, dT.Month, dT.Day, EXP_HOUR_UTC, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime GetTmiExpiration()
        {
            var today = DateTime.UtcNow;
            if (today.Hour < EXP_HOUR_UTC) return SetExpiration(today);
            else
            {
                var tomorrow = today.AddDays(1);
                return SetExpiration(tomorrow);
            }
        }

        public static DateTime GetHistoryExpiration()
        {
            var nextMo = DateTime.UtcNow.AddMonths(1);
            return new DateTime(nextMo.Year, nextMo.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }
    }
}
