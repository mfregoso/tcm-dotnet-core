using Newtonsoft.Json.Linq;

namespace TCM.Web.Interfaces
{
    public interface IClubSearchService
    {
        JObject SearchClubs(string query, int radius, double lat, double lng);
    }
}
