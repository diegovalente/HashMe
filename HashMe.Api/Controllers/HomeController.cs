using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using HashMe.Api.Models;
namespace HashMe.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ReadImage(HttpPostedFileBase file)
        {
            byte[] imageInBytes = null;
            using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
            {
                imageInBytes = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }
            
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:49509")
            };

            // Set the Accept header for BSON.
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/bson"));

            var request = new KeyValuePair<string, byte[]>("image", imageInBytes);

            // POST using the BSON formatter.
            MediaTypeFormatter bsonFormatter = new BsonMediaTypeFormatter();
            var response =  client.PostAsync("api/labels", request, bsonFormatter);
            string labels = string.Empty;
            using (var reader = new BsonReader(await response.Result.Content.ReadAsStreamAsync()))
            {
                reader.ReadRootValueAsArray = true;
                var jsonSerializer = new JsonSerializer();
                var json = jsonSerializer.Deserialize<string[]>(reader);
                labels = ViewBag.ReadImage = string.Join(", ", json);

            }

            //Get Hashtags
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            var responseImage = await client.PostAsJsonAsync<string>("api/hashtags", string.Join(", ", labels)).Result.Content.ReadAsStringAsync();
            var jsonTags = JsonConvert.DeserializeObject<string[]>(responseImage);

            ViewBag.GetImageInstagramHashtags = string.Join(", ", jsonTags);
            
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> GetInstagramHashtags(string keywords)
        {            

            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:49509")
            };

            var response = await client.PostAsJsonAsync<string>("api/hashtags", keywords).Result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<string[]>(response);
            
            ViewBag.GetInstagramHashtags = string.Join(", ", json);
            return View("Index");
        }
    }
}
