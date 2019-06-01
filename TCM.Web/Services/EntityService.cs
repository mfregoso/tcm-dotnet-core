using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;
using TCM.Models.Domain;
using TCM.Web.Entities;
using TCM.Web.Interfaces;

namespace TCM.Web.Services
{
    public class EntityService : IEntityService
    {
        private readonly ClubDataContext _context;
        private readonly IDateHelpers _dateHelpers;
        private readonly IScraperService _scraperService;

        public EntityService(ClubDataContext context, IDateHelpers dateHelpers, IScraperService scraperService)
        {
            _context = context;
            _dateHelpers = dateHelpers;
            _scraperService = scraperService;
        }

        public bool ClubExists(string id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }

        public async Task<ClubInfo> ClubReqHandler(string formattedId)
        {
            var clubResponse = new ClubInfo() { Source = "brand new scrape" };
            var cachedClub = GetClubById(formattedId);

            if (cachedClub != null)
            {
                clubResponse.Info = cachedClub;
                clubResponse.Source = GetDataSourceName();

                if (cachedClub.Exists)
                {
                    bool tmiExpired = _dateHelpers.IsExpired(cachedClub.TMIExpiration);
                    bool historyExpired = _dateHelpers.IsExpired(cachedClub.HistoryExpiration);

                    clubResponse.Source = GetDataSourceName(tmiExpired, historyExpired);
                    cachedClub = await UpdateCachedClubData(cachedClub, tmiExpired, historyExpired);
                
                    return clubResponse;
                }
                else return clubResponse;
            } else
            {
                clubResponse.Info = await NewClubRequest(formattedId);
                return clubResponse;
            }
        }

        public Club GetClubById(string id)
        {
            if (!ClubExists(id)) return null;
            var club = _context.Clubs.Find(id);
            club.MetricsHistory = _context.MetricsHistory.Where(row => row.ClubId == id).ToArray();
            return club;
        }

        public List<ClubStatus> GetAllClubs()
        {
            return _context.Clubs.Select(
                club => new ClubStatus() {
                    Exists = club.Exists,
                    Id = club.Id,
                    MembershipCount = club.MembershipCount
                }).ToList();
        }

        private async Task<Club> NewClubRequest(string formattedId)
        {
            var requestedClub = _scraperService.GetClubStatus(formattedId);
            var newClubEntity = new Club()
            {
                Id = formattedId,
                Exists = requestedClub.Exists,
                MembershipCount = requestedClub.MembershipCount
            };

            if (requestedClub.Exists)
            {
                newClubEntity.TMIExpiration = _dateHelpers.GetTmiExpiration();
                newClubEntity.HistoryExpiration = _dateHelpers.GetHistoryExpiration();
                newClubEntity.MetricsHistory = _scraperService.GetMetricsHistory(formattedId);
            }

            _context.Clubs.Add(newClubEntity);
            await _context.SaveChangesAsync();

            return newClubEntity;
        }

        private async Task<Club> UpdateCachedClubData(Club cachedClub, bool tmiExpired, bool historyExpired)
        {
            if (!tmiExpired && !historyExpired) return cachedClub;
            if (tmiExpired) cachedClub = UpdateCachedClubStatus(cachedClub);
            if (historyExpired) cachedClub = UpdateCachedClubHistory(cachedClub);

            _context.Entry(cachedClub).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return cachedClub;
        }

        private Club UpdateCachedClubStatus(Club cachedClub)
        {
            var currData = _scraperService.GetClubStatus(cachedClub.Id);

            cachedClub.Exists = currData.Exists;
            cachedClub.MembershipCount = currData.MembershipCount;
            cachedClub.TMIExpiration = _dateHelpers.GetTmiExpiration();

            return cachedClub;
        }

        private Club UpdateCachedClubHistory(Club cachedClub)
        {
            cachedClub.MetricsHistory = _scraperService.GetMetricsHistory(cachedClub.Id);
            cachedClub.HistoryExpiration = _dateHelpers.GetHistoryExpiration();
            DeleteClubHistoryFromDb(cachedClub);
            return cachedClub;
        }

        private void DeleteClubHistoryFromDb(Club cachedClub)
        {
            var oldEntries = cachedClub.MetricsHistory.ToList();
            Task.Run(() => _context.MetricsHistory.RemoveRange(oldEntries)).ConfigureAwait(false);
        }

        private string GetDataSourceName(bool tmiExpired = false, bool historyExpired = false)
        {
            if (tmiExpired && !historyExpired)
            {
                return "db+TMIScrape";
            }
            else if (!tmiExpired && historyExpired)
            {
                return "db+historyScrape";
            }
            else if (tmiExpired && historyExpired)
            {
                return "full scrape update";
            }
            else return "database";
        }
    }
}
