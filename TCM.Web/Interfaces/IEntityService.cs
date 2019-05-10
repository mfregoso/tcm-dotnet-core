using System.Collections.Generic;
using TCM.Models;
using TCM.Models.Domain;
using TCM.Models.Entities;

namespace TCM.Web.Interfaces
{
    public interface IEntityService
    {
        bool ClubExists(string id);
        ClubInfo ClubReqHandler(string formattedId);
        List<MetricsHistory> ConvertHistory(string ClubId, List<ClubHistory> clubHistory);
        List<ClubStatus> GetAllClubs();
        Club GetClubById(string id);
    }
}