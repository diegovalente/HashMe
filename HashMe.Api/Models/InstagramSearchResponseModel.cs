using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace HashMe.Api.Models
{
    public class InstagramSearchResponseModel
    {   
        public string status;
        public List<InstagramHashtagsModel> hashtags;
        
    }
    public class InstagramHashtagsModel
    {
        public int position;
        public InstagramHashtagModel hashtag;
    }

    public class InstagramHashtagModel
    {
        public string id;
        [JsonProperty(PropertyName = "media_count")]
        public long mediaCount;
        public string name;
    }
}