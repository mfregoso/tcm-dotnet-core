using System.Collections.Generic;
using TCM.Models.Domain;
using TCM.Web.Entities;

namespace TCM.Web.Interfaces
{
    public interface IScraperService
    {
        ClubStatus GetClubStatus(string id);
        List<MetricsHistory> GetMetricsHistory(string id);
    }
}