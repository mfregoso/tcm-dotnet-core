using System.Collections.Generic;
using System.Threading.Tasks;
using TCM.Models;
using TCM.Models.Domain;
using TCM.Web.Entities;

namespace TCM.Web.Interfaces
{
    public interface IEntityService
    {
        bool ClubExists(string id);
        Task<ClubInfo> ClubReqHandler(string formattedId);
        List<MetricsHistory> ConvertHistory(string ClubId, List<ClubHistory> clubHistory);
        List<ClubStatus> GetAllClubs();
        Club GetClubById(string id);
    }
}