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
            string[] monthYear = { "Jan 09", "feb 11", "Sep 18" };
            //var dates = monthYear.Select(my => DateTime.ParseExact(my, "MMM yy", CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1)).ToArray();
            var dates = monthYear.Select(day => day.Replace(" ", " 20")).ToArray();
            var now = DateTime.UtcNow;
            //System.Diagnostics.Debug.WriteLine(DateHelpers.StaleData(now));
            //System.Diagnostics.Debug.WriteLine(DateHelpers.StaleData(now.AddDays(-1)));
            //System.Diagnostics.Debug.WriteLine(DateHelpers.StaleData(now.AddMonths(-1)));
            return now.ToString();
            return "please get by id: e.g. clubs/455";
        }

        [HttpGet("{id}")]
        public ActionResult<ClubInfo> GetByID(string id)
        {
            bool validId = IdHelpers.IsValid(id);
            ClubInfo clubInfo = new ClubInfo() { Source = "tm" };
            if (validId)
            {
                string formattedId = IdHelpers.FormatId(id);
                DateTime tmiExpiration = DateHelpers.GetTmiExpiration();

                clubInfo.Status = ScraperService.GetClubStatus(formattedId);

                Club newClub = new Club() // Entity Framework
                {
                    Id = formattedId,
                    Exists = clubInfo.Status.Exists,
                    MembershipCount = clubInfo.Status.MembershipCount
                };

                if (clubInfo.Status.Exists)
                {
                    newClub.TMIExpiration = tmiExpiration;
                    newClub.HistoryExpiration = DateHelpers.GetHistoryExpiration();
                    clubInfo.History = ScraperService.GetClubPerformance(formattedId);

                    List<MetricsHistory> metricsHistory = new List<MetricsHistory>();
                    foreach (var item in clubInfo.History)
                    {
                        var temp = new MetricsHistory
                        {
                            ClubId = formattedId,
                            Goals = item.Goals,
                            Members = item.Members,
                            MonthEnd = item.MonthEnd
                        };
                        metricsHistory.Add(temp);
                    }
                    newClub.MetricsHistory = metricsHistory;
                }

                _context.Clubs.Add(newClub);
                _context.SaveChanges();

                return clubInfo;
            }
            else return clubInfo;
        }
    }
}