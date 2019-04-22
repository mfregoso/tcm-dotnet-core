using Microsoft.AspNetCore.Mvc;
using TCM.Services.Utils;

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
            if (validId) return IdHelpers.FormatId(id);
            return id + " is NOT valid!";
        }
    }
}