
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Services
{
    public class AuthManagementClient
    {
        public HttpClient Client {get;}
        public string baseUrl {get;}

        async public static Task<string> GetTokenAsync()
        {
            HttpClient client = new HttpClient();
            var uri = new Uri("https://zapad.eu.auth0.com/oauth/token");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent("{\"client_id\":\"JaruBjn8Ix4GpQe7d0oJzQdfkgb39235\",\"client_secret\":\"1_sEsh8SG_NSBCwtUpPB9-7hcHcObgj1rm-23PwrVVp2BGfxHov7Tlf99KHVmMTT\",\"audience\":\"https://zapad.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);
            var contentResult = response.Content.ReadAsStringAsync().Result;

            var resultDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(contentResult);
            var accessToken = resultDic["access_token"];
            return accessToken;
        }

        public AuthManagementClient(HttpClient client)
        {
            Client = client;
            Client.BaseAddress = new Uri("https://zapad.eu.auth0.com/api/v2/");
        }

        public async Task<UserInfoViewModel> GetUserInfo(string userID)
        {
            var authToken = await GetTokenAsync();

            //var uri = new Uri(baseUrl);
            Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authToken);

            var response = await Client.GetAsync("users/" + userID);
            var result = response.Content.ReadAsStringAsync().Result;

            var realObject = JsonConvert.DeserializeObject<UserInfoViewModel>(result);

            return realObject;
        }

        public async Task<List<string>> GetUsersIds()
        {
            var authToken = await GetTokenAsync();
            Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authToken);

            var response = await Client.GetAsync("users?fields=user_id");
            var result = response.Content.ReadAsStringAsync().Result;

            var objectList = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string,string>>>(result);
            var list = new List<string>();

            foreach(var dict in objectList)
            {
                list.Add(dict["user_id"]);
            }

            return list;
        }

    }
}