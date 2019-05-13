using System;
using TCM.Web.Interfaces;

namespace TCM.Web.Utils
{
    public class DateHelpers : IDateHelpers
    {
        public DateTime GetTmiExpiration()
        {
            var nextWeek = DateTime.UtcNow.AddDays(7);
            return nextWeek;
        }

        public DateTime GetHistoryExpiration()
        {
            var nextMo = DateTime.UtcNow.AddMonths(1);
            return new DateTime(nextMo.Year, nextMo.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public bool IsExpired(DateTime? expirationDate)
        {
            return DateTime.UtcNow > expirationDate ? true : false;
        }
        
        //string[] monthYear = { "Jan 09", "feb 11", "Sep 18" };
        //var dates = monthYear.Select(my => DateTime.ParseExact(my, "MMM yy", CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1)).ToArray();
    }
}
