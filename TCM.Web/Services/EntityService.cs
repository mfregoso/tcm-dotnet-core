﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TCM.Models;
using TCM.Models.Domain;
using TCM.Models.Entities;
using TCM.Web.Interfaces;

namespace TCM.Web.Services
{
    public class EntityService : IEntityService
    {
        private ClubDataContext _context { get; }
        private readonly IDateHelpers _dateHelpers;

        public EntityService(ClubDataContext context, IDateHelpers dateHelpers)
        {
            _context = context;
            _dateHelpers = dateHelpers;
        }

        public bool ClubExists(string id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }

        public ClubInfo ClubReqHandler(string formattedId)
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
            List<MetricsHistory> metricsHistory = new List<MetricsHistory>();
            if (clubHistory.Count == 0) return metricsHistory;

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
                newClubEntity.TMIExpiration = _dateHelpers.GetTmiExpiration();
                newClubEntity.HistoryExpiration = _dateHelpers.GetHistoryExpiration();
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
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return cachedClub;
        }

        private Club UpdateCachedClubStatus(Club cachedClub)
        {
            var currData = ScraperService.GetClubStatus(cachedClub.Id);

            cachedClub.Exists = currData.Exists;
            cachedClub.MembershipCount = currData.MembershipCount;
            cachedClub.TMIExpiration = _dateHelpers.GetTmiExpiration();

            return cachedClub;
        }

        private Club UpdateCachedClubHistory(Club cachedClub)
        {
            string formattedId = cachedClub.Id;
            var updatedHistory = ScraperService.GetMetricsHistory(formattedId);
            cachedClub.HistoryExpiration = _dateHelpers.GetHistoryExpiration();

            DeleteClubHistoryFromDb(cachedClub);
            cachedClub.MetricsHistory = ConvertHistory(formattedId, updatedHistory);

            return cachedClub;
        }

        private void DeleteClubHistoryFromDb(Club cachedClub)
        {
            var oldEntries = cachedClub.MetricsHistory.ToList();
            _context.MetricsHistory.RemoveRange(oldEntries);
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