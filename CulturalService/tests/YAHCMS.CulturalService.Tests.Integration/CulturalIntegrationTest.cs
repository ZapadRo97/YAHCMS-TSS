using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using RestSharp;
using Xunit;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Tests.Integration
{
    public class SimpleIntegrationTest
    {

        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        private readonly Artist someArtist;
        private readonly Location someLocation;
        private readonly Quiz someQuiz;

        public SimpleIntegrationTest()
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<YAHCMS.CulturalService.Startup>().UseUrls("http://*:5007"));
            testClient = testServer.CreateClient();
            testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());



            var date = DateTime.Now;
            someArtist = new Artist();
            someArtist.Name = "name";
            someArtist.Description = "desc";
            someArtist.LocationID = 1;
            someArtist.BirthDate = date;
            someArtist.DeathDate = date;
            someArtist.PhotoName = "photo";
            someLocation = new Location();
            someLocation.Name = "name";
            someLocation.Description = "desc";
            someLocation.Latitude = 11;
            someLocation.Longitude = 12;
            someLocation.PhotoName = "photo";
            someLocation.Artists = new List<Artist>();
            someQuiz = new Quiz();
            someQuiz.Title = "title";
            someQuiz.Description = "desc";
            someQuiz.Questions = new List<QuizQuestion>();
        }

        public string GetAccessToken()
        {
            var client = new RestClient("https://zapad.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"cid\",\"client_secret\":\"secr\",\"audience\":\"yahcms\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var jsonString = response.Content;
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            var token = json["access_token"];
            return token;
        }

        public async Task<Artist> AddArtist()
        {

            StringContent stringContent = new StringContent(
                JsonConvert.SerializeObject(someArtist),
                UnicodeEncoding.UTF8, "application/json");

            var postResponse =
                await testClient.PostAsync("http://localhost:5007/api/artists", stringContent);
            postResponse.EnsureSuccessStatusCode();
            string raw = await postResponse.Content.ReadAsStringAsync();

            Artist artist = JsonConvert.DeserializeObject<Artist>(raw);
            return artist;
        }

        [Fact]
        public async void AddArtistAndGetList()
        {

            await AddArtist();

            var getResponse = await testClient.GetAsync("/api/artists/");
            getResponse.EnsureSuccessStatusCode();

            string raw = await getResponse.Content.ReadAsStringAsync();
            List<Artist> artists =
                JsonConvert.DeserializeObject<List<Artist>>(raw);

            Assert.NotEqual(0, artists.Count());
            Assert.NotEqual(0, artists[0].ID);

        }

        public async Task<Location> AddLocation()
        {

            StringContent stringContent = new StringContent(
                JsonConvert.SerializeObject(someLocation),
                UnicodeEncoding.UTF8, "application/json");

            var postResponse =
                await testClient.PostAsync("http://localhost:5007/api/locations", stringContent);
            postResponse.EnsureSuccessStatusCode();
            string raw = await postResponse.Content.ReadAsStringAsync();

            Location location = JsonConvert.DeserializeObject<Location>(raw);
            return location;
        }

        
        [Fact]
        public async void DeleteLocationCheck()
        {
            Location location = await AddLocation();

            var getResponse = await testClient.GetAsync("/api/locations");
                getResponse.EnsureSuccessStatusCode();

            string raw = await getResponse.Content.ReadAsStringAsync();

            List<Location> locationsBefore =
                JsonConvert.DeserializeObject<List<Location>>(raw);

            var deleteResponse = await testClient.DeleteAsync($"api/locations/{location.ID}");
            deleteResponse.EnsureSuccessStatusCode();

            getResponse = await testClient.GetAsync("/api/locations");
                getResponse.EnsureSuccessStatusCode();

            raw = await getResponse.Content.ReadAsStringAsync();

            List<Location> locationsAfter =
                JsonConvert.DeserializeObject<List<Location>>(raw);

            Assert.Equal(locationsBefore.Count, locationsAfter.Count + 1);
        }
    }
}