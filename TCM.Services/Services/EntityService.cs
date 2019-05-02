using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TCM.Models;
using TCM.Models.Domain;
using TCM.Models.Entities;
using TCM.Services.Utils;

namespace TCM.Services
{
    public class EntityService
    {
        private ClubDataContext _context { get; }

        public EntityService(ClubDataContext context)
        {
            _context = context;
        }

        public bool ClubExists(string id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }

        public ClubInfo ClubReqHandler(string formattedId)
        {
            var clubResponse = new ClubInfo() { Source = "full scrape" };
            var cachedClub = GetClubById(formattedId);

            if (cachedClub != null)
            {
                clubResponse.Info = cachedClub;
                clubResponse.Source = "database";

                if (cachedClub.Exists)
                {
                    bool tmiExpired = DateHelpers.IsExpired(cachedClub.TMIExpiration);
                    bool historyExpired = DateHelpers.IsExpired(cachedClub.HistoryExpiration);

                    clubResponse.Source = GetDataSourceName(tmiExpired, historyExpired);
                    cachedClub = UpdateCachedClubData(cachedClub, tmiExpired, historyExpired);
                
                    return clubResponse;
                }
                else return clubResponse;
            } else
            {
                clubResponse.Info = NewClubRequest(formattedId);
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

        public List<MetricsHistory> ConvertHistory(string ClubId, List<ClubHistory> clubHistory)
        {
            if (clubHistory.Count == 0) return null;
            List<MetricsHistory> metricsHistory = new List<MetricsHistory>();
            foreach (var item in clubHistory)
            {
                var temp = new MetricsHistory
                {
                    ClubId = ClubId,
                    Goals = item.Goals,
                    Members = item.Members,
                    MonthEnd = item.MonthEnd
                };
                metricsHistory.Add(temp);
            }
            return metricsHistory;
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

        private Club NewClubRequest(string formattedId)
        {
            var requestedClub = ScraperService.GetClubStatus(formattedId);
            var newClubEntity = new Club()
            {
                Id = formattedId,
                Exists = requestedClub.Exists,
                MembershipCount = requestedClub.MembershipCount
            };

            if (requestedClub.Exists)
            {
                newClubEntity.TMIExpiration = DateHelpers.GetTmiExpiration();
                newClubEntity.HistoryExpiration = DateHelpers.GetHistoryExpiration();
                var mHistory = ScraperService.GetMetricsHistory(formattedId);
                newClubEntity.MetricsHistory = ConvertHistory(formattedId, mHistory);
            }

            _context.Clubs.Add(newClubEntity);
            _context.SaveChanges();

            return newClubEntity;
        }

        private Club UpdateCachedClubData(Club cachedClub, bool tmiExpired, bool historyExpired)
        {
            if (!tmiExpired && !historyExpired) return cachedClub;
            if (tmiExpired) cachedClub = UpdateCachedClubStatus(cachedClub);
            if (historyExpired) cachedClub = UpdateCachedClubHistory(cachedClub);

            _context.Entry(cachedClub).State = EntityState.Modified;
            _context.SaveChanges();
            return cachedClub;
        }

        private Club UpdateCachedClubStatus(Club cachedClub)
        {
            var currData = ScraperService.GetClubStatus(cachedClub.Id);

            cachedClub.Exists = currData.Exists;
            cachedClub.MembershipCount = currData.MembershipCount;
            cachedClub.TMIExpiration = DateHelpers.GetTmiExpiration();

            return cachedClub;
        }

        private Club UpdateCachedClubHistory(Club cachedClub)
        {
            string formattedId = cachedClub.Id;
            var updatedHistory = ScraperService.GetMetricsHistory(formattedId);
            cachedClub.HistoryExpiration = DateHelpers.GetHistoryExpiration();

            DeleteClubHistoryFromDb(cachedClub);
            cachedClub.MetricsHistory = ConvertHistory(formattedId, updatedHistory);

            return cachedClub;
        }

        private void DeleteClubHistoryFromDb(Club cachedClub)
        {
            var oldEntries = cachedClub.MetricsHistory.ToList();
            _context.MetricsHistory.RemoveRange(oldEntries);
        }

        private string GetDataSourceName(bool tmiExpired, bool historyExpired)
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
                return "full scrape";
            }
            else return "database";
        }
    }
}
