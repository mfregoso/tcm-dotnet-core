using System;
using System.Collections.Generic;
using System.Text;
using TCM.Models.Domain;

namespace TCM.Models
{
    public class ClubInfo
    {
        public string Source { get; set; }
        public ClubStatus Status { get; set; }
        public List<ClubHistory> History { get; set; }
    }
}
