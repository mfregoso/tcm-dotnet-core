namespace TCM.Models.Domain
{
    public class ClubStatus
    {
        public string Id { get; set; }
        public bool Exists { get; set; }
        public int? MembershipCount { get; set; }

        public override string ToString()
        {
            return "\r\nClub Id: " + Id + "\r\nExists: " + Exists + "\r\nMembers: " + MembershipCount;
        }
    }
}
