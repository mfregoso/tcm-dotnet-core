using AngleSharp.Html.Parser;
using System;
using System.Net.Http;
using TCM.Models;

namespace TCM.Services
{
    public class ScraperService
    {
        public static ClubStatus GetClubStatus(string id)
        {
            string BaseUrl = "http://dashboards.toastmasters.org/ClubReport.aspx?id=";
            var clubStatus = new ClubStatus();

            using (var client = new HttpClient())
            {
                try
                {
                    var clubReport = client.GetStreamAsync(BaseUrl + id).Result;
                    var parseHtml = new HtmlParser();
                    var dataTable = parseHtml.ParseDocument(clubReport);
                    var statusBox = dataTable.QuerySelector(".tabBody > center > span");
                    if (statusBox == null) return clubStatus;
                    else
                    {
                        clubStatus.Exists = true;
                        clubStatus.IsActive = !statusBox.TextContent.Contains("Suspended");
                        var dataColumn = dataTable.QuerySelectorAll("table.clubStatusChart")[1];
                        var dataRow = dataColumn.QuerySelectorAll("table tr")[1];
                        var data = dataRow.QuerySelectorAll("td.chart_table_big_numbers")[1];
                        clubStatus.MembershipCount = int.TryParse(data.TextContent, out int mCt) ? mCt : (int?)null;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            return clubStatus;
        }
    }
}
