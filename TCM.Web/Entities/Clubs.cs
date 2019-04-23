using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TCM.Models.Domain;

namespace TCM.Web.Entities
{
    public class Clubs
    {
        public class ClubDataContext : DbContext
        {
            public ClubDataContext(DbContextOptions<ClubDataContext> options)
                : base(options)
            { }

            public DbSet<Club> Clubs { get; set; }
            public DbSet<MetricsHistory> MetricsHistory { get; set; }
        }

        public class Club : ClubStatus
        {
            [Key]
            public override string Id { get; set; }
            public DateTime? StatusExpiration { get; set; }
            public DateTime? HistoryExpiration { get; set; }

            public ICollection<MetricsHistory> MetricsHistory { get; set; }
        }

        public class MetricsHistory : ClubHistory
        {
            [Key]
            public int Id { get; set; }
            public string ClubId { get; set; }
        }
    }
}
