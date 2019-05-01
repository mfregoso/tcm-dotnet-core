using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TCM.Services
{
    public class ClubSearchService
    {
        public static JObject SearchClubs(string query, int radius, double lat, double lng)
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
                var errMsg = "{\"error\": true, \"message\": \"" + ex.Message + "\"}";
                clubs = (JObject)JsonConvert.DeserializeObject(errMsg);
            }
            return clubs;
        }
    }
}
