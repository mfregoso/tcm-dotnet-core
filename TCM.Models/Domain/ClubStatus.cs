namespace TCM.Models.Domain
{
    public class ClubStatus
    {
        public bool Exists { get; set; }
        public bool IsActive { get; set; }
        public int? MembershipCount { get; set; }

        public override string ToString()
        {
            return "Exists: " + Exists + "\r\nActive: " + IsActive + "\r\nMembers: " + MembershipCount;
        }
    }
}
