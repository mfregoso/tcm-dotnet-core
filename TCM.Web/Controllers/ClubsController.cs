using Microsoft.AspNetCore.Mvc;
using TCM.Services;
using TCM.Services.Utils;
using TCM.Models.Domain;
using System.Collections.Generic;

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
                ClubStatus clubStatus = ScraperService.GetClubStatus(formattedId);
                if (clubStatus.Exists)
                {
                    List<ClubPerformance> clubPerfHistory = ScraperService.GetClubPerformance(formattedId);
                    foreach(var club in clubPerfHistory)
                    {
                        System.Diagnostics.Debug.WriteLine(club.MonthEnd + " " + club.Members);
                    }
                }

                return clubStatus.ToString();
            }
            else return "Invalid ID!";
        }
    }
}