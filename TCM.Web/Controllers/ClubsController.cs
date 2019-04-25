using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using TCM.Models;
using TCM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using TCM.Services;
using TCM.Services.Utils;
using System.Collections.Generic;
using TCM.Models.Domain;

namespace TCM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ClubDataContext _context;

        public ClubsController(ClubDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            //string[] monthYear = { "Jan 09", "feb 11", "Sep 18" };
            //var dates = monthYear.Select(my => DateTime.ParseExact(my, "MMM yy", CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1)).ToArray();
            return "please get by id: e.g. clubs/455";
        }

        [HttpGet("{id}")]
        public ActionResult<ClubInfo> GetByID(string id)
        {
            ClubInfo clubInfo = new ClubInfo();
            if (!IdHelpers.IsValid(id)) return clubInfo;

            string formattedId = IdHelpers.FormatId(id);
            var cachedClub = GetClubById(formattedId);
            bool fullClubUpdate = false;

            if (cachedClub != null)
            {
                clubInfo.Source = "database";
                clubInfo.Status = cachedClub;

                if (cachedClub.Exists)
                {
                    bool tmiExpired = DateHelpers.IsExpired(cachedClub.TMIExpiration);
                    bool historyExpired = DateHelpers.IsExpired(cachedClub.HistoryExpiration);
                    if (tmiExpired && !historyExpired)
                    {
                        clubInfo.Source = "db+TMIScrape";
                        cachedClub.TMIExpiration = DateHelpers.GetTmiExpiration();
                        var currClub = ScraperService.GetClubStatus(formattedId);
                        cachedClub.MembershipCount = currClub.MembershipCount;

                        _context.Entry(cachedClub).State = EntityState.Modified;
                        _context.SaveChanges();
                        return clubInfo;
                    }
                    else if (tmiExpired && historyExpired)
                    {
                        fullClubUpdate = true;
                    }
                    else if (!tmiExpired && !historyExpired)
                    {
                        return clubInfo;
                    }
                }
                else return clubInfo;
            }

            DateTime tmiExpiration = DateHelpers.GetTmiExpiration();
            clubInfo.Status = ScraperService.GetClubStatus(formattedId);
            clubInfo.Source = "scrape";

            Club newClubEntity = new Club() // Entity Framework
            {
                Id = formattedId,
                Exists = clubInfo.Status.Exists,
                MembershipCount = clubInfo.Status.MembershipCount
            };

            if (clubInfo.Status.Exists)
            {
                newClubEntity.TMIExpiration = tmiExpiration;
                newClubEntity.HistoryExpiration = DateHelpers.GetHistoryExpiration();
                clubInfo.History = ScraperService.GetClubPerformance(formattedId);
                newClubEntity.MetricsHistory = ConvertHistory(formattedId, clubInfo.History);
            }

            if (fullClubUpdate)
            {
                _context.Entry(newClubEntity).State = EntityState.Modified;
                _context.SaveChanges();
            } else
            {
                _context.Clubs.Add(newClubEntity);
                _context.SaveChanges();
            }

            return clubInfo;
        }

        private bool ClubExists(string id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }

        private Club GetClubById(string id)
        {
            if (!ClubExists(id)) return null;
            var club = _context.Clubs.Find(id);
            club.MetricsHistory = _context.MetricsHistory.Where(row => row.ClubId == id).ToArray();
            return club;
        }

        private List<MetricsHistory> ConvertHistory(string ClubId, List<ClubHistory> clubHistory)
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
    }
}