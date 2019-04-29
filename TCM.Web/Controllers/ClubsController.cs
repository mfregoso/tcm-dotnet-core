using Microsoft.AspNetCore.Mvc;
using TCM.Models;
using TCM.Models.Entities;
using TCM.Services;
using TCM.Services.Utils;

namespace TCM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ClubDataContext _context;
        private readonly EntityService entityService;

        public ClubsController(ClubDataContext context)
        {
            _context = context;
            entityService = new EntityService(_context);
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            //string[] monthYear = { "Jan 09", "feb 11", "Sep 18" };
            //var dates = monthYear.Select(my => DateTime.ParseExact(my, "MMM yy", CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1)).ToArray();
            //return entityService.GetAllClubs(); //List<ClubStatus>
            return "please get by id: e.g. clubs/455";
        }

        [HttpGet("{id}")]
        public ActionResult<ClubInfo> GetByID(string id)
        {
            if (!IdHelpers.IsValid(id)) return new ClubInfo();
            string formattedId = IdHelpers.FormatId(id);

            return entityService.ClubReqHandler(formattedId);
        }
    }
}