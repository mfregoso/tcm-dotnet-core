﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using TCM.Web.Interfaces;

namespace TCM.Web.Services
{
    public class ClubSearchService : IClubSearchService
    {
        public JObject SearchClubs(string query, int radius, double lat, double lng)
        {
            string clubSearchAPI = "http://www.toastmasters.org/api/sitecore/FindAClub/Search?q=" + query + "&radius=" + radius + "&n=&status=O&advanced=0&latitude=" + lat + "&longitude=" + lng + "&autocomplete=false";

            HttpWebRequest getClubs = (HttpWebRequest)WebRequest.Create(clubSearchAPI);
            getClubs.Method = "GET";
            getClubs.ContentType = "application/x-www-form-urlencoded";
            getClubs.ContentLength = 0;
            string nearbyClubs = "";
            JObject clubs = new JObject();

            try
            {
                using (HttpWebResponse webResp = (HttpWebResponse)getClubs.GetResponse())
                {
                    using (Stream stream = webResp.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        nearbyClubs = reader.ReadToEnd();
                    }
                    clubs = (JObject)JsonConvert.DeserializeObject(nearbyClubs);
                }
            }
            catch (WebException ex)
            {
                var errMsg = new { error = true, message = ex.Message };
                clubs = JObject.FromObject(errMsg);
            }
            return clubs;
        }
    }
}
