using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using HashMe.Api.Models;

namespace HashMe.Api.Providers
{
    public static class HashtagProvider
    {
        /// <summary>
        /// Get top instagram hasgtags based on keywords.
        /// </summary>
        /// <returns>A list of instagram Hashtags</returns>
        public static async Task<List<string>> GetInstagramHashtags(string keywords)
        {
            List<InstagramHashtagModel> instagramHashtags = new List<InstagramHashtagModel>();
            List<string> hashtags = new List<string>();

            using (var client = new HttpClient())
            {                
                foreach (var keyword in keywords.Trim().Split(','))
                {
                    var response = await client.GetAsync("https://www.instagram.com/web/search/topsearch/?query=%23"+keyword).Result.Content.ReadAsStringAsync();
                    
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

                    var responseModel = JsonConvert.DeserializeObject<InstagramSearchResponseModel>(response, jsonSerializerSettings);
                    instagramHashtags.AddRange(responseModel.hashtags.Select(a => a.hashtag));
                }
            }
            
            return instagramHashtags.OrderByDescending(o => o.mediaCount).Take(30).Select(h => "#"+h.name).ToList();            
        }
    }
}