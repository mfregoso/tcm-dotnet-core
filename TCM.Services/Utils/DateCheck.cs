using System;
using TCM.Models.Domain;

namespace TCM.Services.Utils
{
    public class DateCheck
    {
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
