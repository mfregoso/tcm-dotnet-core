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
            ClubInfo clubResponse = new ClubInfo();
            var cachedClub = GetClubById(formattedId);
            bool fullClubUpdate = false;

            if (cachedClub != null)
            {
                clubResponse.Source = "database";
                clubResponse.Info = cachedClub;

                if (cachedClub.Exists)
                {
                    bool tmiExpired = DateHelpers.IsExpired(cachedClub.TMIExpiration);
                    bool historyExpired = DateHelpers.IsExpired(cachedClub.HistoryExpiration);
                    if (tmiExpired && !historyExpired)
                    {
                        clubResponse.Source = "db+TMIScrape";
                        cachedClub.TMIExpiration = DateHelpers.GetTmiExpiration();
                        var currClub = ScraperService.GetClubStatus(formattedId);
                        cachedClub.MembershipCount = currClub.MembershipCount;

                        _context.Entry(cachedClub).State = EntityState.Modified;
                        _context.SaveChanges();
                        return clubResponse;
                    }
                    else if (tmiExpired && historyExpired)
                    {
                        fullClubUpdate = true;
                    }
                    else if (!tmiExpired && !historyExpired)
                    {
                        return clubResponse;
                    }
                    else if (!tmiExpired && historyExpired)
                    {
                        clubResponse.Source = "db+historyScrape";
                        cachedClub.HistoryExpiration = DateHelpers.GetHistoryExpiration();
                        var updatedHistory = ScraperService.GetMetricsHistory(formattedId);

                        var oldEntries = cachedClub.MetricsHistory.ToList();
                        _context.MetricsHistory.RemoveRange(oldEntries);

                        cachedClub.MetricsHistory = ConvertHistory(formattedId, updatedHistory);
                        _context.Entry(cachedClub).State = EntityState.Modified;
                        _context.SaveChanges();

                        return clubResponse;
                    }
                }
                else return clubResponse;
            }

            var clubStatus = ScraperService.GetClubStatus(formattedId);
            clubResponse.Source = "full scrape";

            Club newClubEntity = new Club() // Entity Framework
            {
                Id = formattedId,
                Exists = clubStatus.Exists,
                MembershipCount = clubStatus.MembershipCount
            };

            if (clubStatus.Exists)
            {
                newClubEntity.TMIExpiration = DateHelpers.GetTmiExpiration(); ;
                newClubEntity.HistoryExpiration = DateHelpers.GetHistoryExpiration();
                var mHistory = ScraperService.GetMetricsHistory(formattedId);
                newClubEntity.MetricsHistory = ConvertHistory(formattedId, mHistory);
            }

            if (fullClubUpdate)
            {
                _context.Entry(newClubEntity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                _context.Clubs.Add(newClubEntity);
                _context.SaveChanges();
            }

            clubResponse.Info = newClubEntity;
            return clubResponse;
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
    }
}
