using Microsoft.AspNetCore.Mvc;
using TCM.Services;
using TCM.Services.Utils;
using TCM.Models;

namespace TCM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "please get by id: e.g. clubs/455";
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetByID(string id)
        {
            bool validId = IdHelpers.IsValid(id);
            if (validId)
            {
                string formattedId = IdHelpers.FormatId(id);
                ClubStatus currMembers = ScraperService.GetClubStatus(formattedId);
                return currMembers.ToString();
            }
            else return new ClubStatus().ToString();
        }
    }
}