using System;
using System.Collections.Generic;
using System.Text;

namespace TCM.Models.Domain
{
    public class DataCurrentStatus
    {
        public bool StaleStatus { get; set; }
        public bool StaleHistory { get; set; }

        public override string ToString()
        {
            return "Stale Club TMI: " + StaleStatus + "\r\nStale History: " + StaleHistory;
        }
    }
}
