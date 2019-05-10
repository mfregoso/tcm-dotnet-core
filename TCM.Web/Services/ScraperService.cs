using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TCM.Models.Domain;

namespace TCM.Web.Services
{
    public class ScraperService
    {
        public static ClubStatus GetClubStatus(string id)
        {
            string BaseUrl = "http://dashboards.toastmasters.org/ClubReport.aspx?id=";
            var clubStatus = new ClubStatus() { Id = id };

            using (var client = new HttpClient())
            {
                try
                {
                    var clubReport = client.GetStreamAsync(BaseUrl + id).Result;
                    var parseHtml = new HtmlParser();
                    var dataTable = parseHtml.ParseDocument(clubReport);
                    var statusBox = dataTable.QuerySelector(".tabBody > center > span");
                    if (statusBox == null || statusBox.TextContent.Contains("Suspended")) return clubStatus;
                    else
                    {
                        clubStatus.Exists = true;
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

        public static List<ClubHistory> GetMetricsHistory(string id)
        {
            string lastMo = DateTime.Now.AddMonths(-1).Month.ToString();
            string BaseUrl = "https://www.marshalls.org/tmtools/DCP_Hist.cgi?mon=" + lastMo + "&club=";

            using (var client = new HttpClient())
            {
                List<ClubHistory> clubHistory = new List<ClubHistory>();
                try
                {
                    var tmTools = client.GetStringAsync(BaseUrl + id).Result;
                    var parseHtml = new HtmlParser();
                    var dataTable = parseHtml.ParseDocument(tmTools);
                    var dataRows = dataTable.QuerySelectorAll("table")[1];
                    var data = dataRows.QuerySelectorAll("tr").Skip(3);
                    foreach (var row in data)
                    {
                        ClubHistory metrics = new ClubHistory();
                        bool hasMembers = int.TryParse(row.ChildNodes[7].TextContent, out int members);
                        bool hasGoals = int.TryParse(row.ChildNodes[8].TextContent, out int goals);
                        metrics.MonthEnd = row.ChildNodes[0].TextContent;
                        metrics.Members = hasMembers ? members : (int?)null;
                        metrics.Goals = hasGoals ? goals : (int?)null;

                        clubHistory.Add(metrics);
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("tmtools server error");
                }
                return clubHistory;
            }
        }
    }
}
