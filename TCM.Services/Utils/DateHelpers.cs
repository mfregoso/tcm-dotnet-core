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

        public static DateTime GetStatusExpiration()
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

        public static DataCurrentStatus StaleData(DateTime lastAccessed) // use expiration instead?
        {
            var now = DateTime.UtcNow;
            var nowBuffer = now.AddHours(-24);
            var hoursDiff = now.Subtract(lastAccessed).TotalHours;
            bool isStatusStale = hoursDiff >= 23 || now.Day > lastAccessed.Day && now.Hour >= 18 ? true : false;
            bool isHistoryStale = nowBuffer.Month > lastAccessed.Month ? true : false;
            return new DataCurrentStatus { StaleStatus = isStatusStale, StaleHistory = isHistoryStale };
        }
        public static bool NewDailyData(DateTime lastAccessed)
        {
            var now = DateTime.UtcNow;
            return now.Day > lastAccessed.Day && now.Hour >= 18 ? true : false;
        }

        public static bool NewHistoricalData(DateTime lastAccessed)
        {
            var nowBuffer = DateTime.UtcNow.AddHours(-24);
            return nowBuffer.Month > lastAccessed.Month ? true : false;
        }
    }
}
