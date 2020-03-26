using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using RestSharp;
using Xunit;
using YAHCMS.UserService.Models;
using YAHCMS.UserService;

namespace YAHCMS.UserService.Tests.Integration
{
    public class UserIntegrationTest
    { 
        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        private readonly User someUser;

        public UserIntegrationTest()
        {

            
            testServer = new TestServer(new  WebHostBuilder().UseStartup<YAHCMS.UserService.Startup>().UseUrls("http://*:5005"));
            testClient = testServer.CreateClient();
            testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
            


            someUser = new User();
            someUser.UID = System.Guid.NewGuid().ToString();
            someUser.Name = "Marcelino";
            someUser.Score = 7;
        }

        public string GetAccessToken() {
            var client = new RestClient("https://zapad.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"clid\",\"client_secret\":\"secr\",\"audience\":\"yahcms\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var jsonString = response.Content;
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            var token = json["access_token"];
            return token;
        }

        public async Task<User> AddUser()
        {

            StringContent stringContent = new StringContent(
                JsonConvert.SerializeObject(someUser), 
                UnicodeEncoding.UTF8, "application/json");

            var postResponse = 
                await testClient.PostAsync("http://localhost:5005/api/users", stringContent);
            postResponse.EnsureSuccessStatusCode();
            string raw = await postResponse.Content.ReadAsStringAsync();

            User user = JsonConvert.DeserializeObject<User>(raw);
            return user;
        }

        [Fact]
        public async void AddAndGetUser()
        {

            await AddUser();

            var getResponse = await testClient.GetAsync("/api/users/abc");
                getResponse.EnsureSuccessStatusCode();

            string raw = await getResponse.Content.ReadAsStringAsync();

            User user1 =
                JsonConvert.DeserializeObject<User>(raw);

                Assert.Equal("Marcelino", user1.Name);
        
        }

    }
}