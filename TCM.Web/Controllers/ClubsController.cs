using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using TCM.Models;
using TCM.Services;
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
        public ActionResult<ClubInfo> GetByID(string id)
        {
            bool validId = IdHelpers.IsValid(id);
            ClubInfo clubInfo = new ClubInfo() { Source = "tm" };
            if (validId)
            {
                string formattedId = IdHelpers.FormatId(id);
                clubInfo.Status = ScraperService.GetClubStatus(formattedId);
                if (clubInfo.Status.Exists)
                {
                    clubInfo.History = ScraperService.GetClubPerformance(formattedId);
                }

                return clubInfo;
            }
            else return clubInfo;
        }
    }
}