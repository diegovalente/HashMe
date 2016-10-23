using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HashMe.Api.Providers;


namespace HashMe.Api.Controllers
{
    public class HashtagsController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new List<string>() { "Nothing here." };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/values
        public IEnumerable<string> Post([FromBody]string keywords)
        {
            var hashtags = HashtagProvider.GetInstagramHashtags(keywords).Result;

            return hashtags;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
