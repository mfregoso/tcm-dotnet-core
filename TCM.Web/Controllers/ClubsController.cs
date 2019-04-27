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
        private readonly EntityService entityService;

        public ClubsController(ClubDataContext context)
        {
            _context = context;
            entityService = new EntityService(_context);
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            //string[] monthYear = { "Jan 09", "feb 11", "Sep 18" };
            //var dates = monthYear.Select(my => DateTime.ParseExact(my, "MMM yy", CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1)).ToArray();
            //return entityService.GetAllClubs(); //List<ClubStatus>
            return "please get by id: e.g. clubs/455";
        }

        [HttpGet("{id}")]
        public ActionResult<ClubInfo> GetByID(string id)
        {
            ClubInfo clubResponse = new ClubInfo();
            if (!IdHelpers.IsValid(id)) return clubResponse;

            string formattedId = IdHelpers.FormatId(id);
            var cachedClub = entityService.GetClubById(formattedId);
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
                        var updatedHistory = ScraperService.GetClubPerformance(formattedId);

                        var oldEntries = cachedClub.MetricsHistory.ToList();
                        _context.MetricsHistory.RemoveRange(oldEntries);

                        cachedClub.MetricsHistory = entityService.ConvertHistory(formattedId, updatedHistory);
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
                var mHistory = ScraperService.GetClubPerformance(formattedId);
                newClubEntity.MetricsHistory = entityService.ConvertHistory(formattedId, mHistory);
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

            clubResponse.Info = newClubEntity;
            return clubResponse;
        }
    }
}