using System;

namespace TCM.Web.Interfaces
{
    public interface IDateHelpers
    {
        DateTime GetHistoryExpiration();
        DateTime GetTmiExpiration();
        bool IsExpired(DateTime? expirationDate);
    }
}