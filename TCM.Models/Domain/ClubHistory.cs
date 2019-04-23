using System.ComponentModel.DataAnnotations;

namespace TCM.Models.Domain
{
    public class ClubHistory
    {
        [MaxLength(20)]
        public string MonthEnd { get; set; }
        public int? Members { get; set; }
        public int? Goals { get; set; }
    }
}
