using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TCM.Models;
using TCM.Web.Interfaces;
using TCM.Web.Utils;

namespace TCM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IEntityService _entityService;
        private readonly IClubSearchService _clubSearchService;

        public ClubsController(IEntityService entityService, IClubSearchService clubSearchService)
        {
            _entityService = entityService;
            _clubSearchService = clubSearchService;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "please get by id: e.g. clubs/455";
        }

        [HttpGet("{id}")]
        public ActionResult<ClubInfo> GetByID(string id)
        {
            if (!IdHelpers.IsValid(id)) return BadRequest(new { message = "Invalid club number" }); //NotFound()?
            string formattedId = IdHelpers.FormatId(id);

            return _entityService.ClubReqHandler(formattedId);
        }

        [HttpGet("search")]
        public ActionResult<JObject> SearchClubs(string query, int radius, double latitude, double longitude)
        {
            return _clubSearchService.SearchClubs(query, radius, latitude, longitude);
        }
    }
}