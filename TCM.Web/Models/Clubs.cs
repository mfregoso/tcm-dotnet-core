using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TCM.Models.Domain;

namespace TCM.Web.Models
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
        public DateTime TMIExpiration { get; set; }
        public DateTime HistoryExpiration { get; set; }
        public ICollection<MetricsHistory> MetricsHistory { get; set; }
    }

    public class MetricsHistory : ClubHistory
    {
        public int Id { get; set; }
        [StringLength(8)]
        public string ClubId { get; set; }
    }
}
