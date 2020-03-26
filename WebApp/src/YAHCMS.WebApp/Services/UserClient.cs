
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Services
{
    public class UserClient
    {
        public HttpClient Client {get;}

        private readonly IOptions<ApiEndpoints> _apiEndpoints;

        public UserClient(HttpClient client, 
            IOptions<ApiEndpoints> apiEndpoints)
        {
            Client = client;
            _apiEndpoints = apiEndpoints;
            client.BaseAddress = new Uri(_apiEndpoints.Value.UserApi);
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            var response = await Client.GetAsync("users");
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();

            var allUsers = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(stringContent);
            return allUsers;
        }

        public async Task<bool> CreateAsync(UserViewModel user)
        {
            var jsonData = JsonConvert.SerializeObject(user);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("users", data);

            return response.IsSuccessStatusCode;
        }



        
    }
}