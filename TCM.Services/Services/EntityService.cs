using System.Collections.Generic;
using System.Linq;
using TCM.Models.Domain;
using TCM.Models.Entities;

namespace TCM.Services
{
    public class EntityService
    {
        public EntityService(ClubDataContext context)
        {
            _context = context;
        }

        private ClubDataContext _context { get; }

        public bool ClubExists(string id)
        {
            return _context.Clubs.Any(e => e.Id == id);
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
