using System.Collections.Generic;
using System.Threading.Tasks;
using TCM.Models.Domain;
using TCM.Web.Entities;

namespace TCM.Web.Interfaces
{
    public interface IScraperService
    {
        Task<ClubStatus> GetClubStatus(string id);
        Task<List<MetricsHistory>> GetMetricsHistory(string id);
    }
}