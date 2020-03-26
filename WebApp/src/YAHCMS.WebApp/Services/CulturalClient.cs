

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Services
{
    public class CulturalClient
    {
        public HttpClient Client {get;}

        private readonly IOptions<ApiEndpoints> _apiEndpoints;

        public CulturalClient(HttpClient client, 
            IOptions<ApiEndpoints> apiEndpoints)
        {
            Client = client;
            _apiEndpoints = apiEndpoints;
            client.BaseAddress = new Uri(_apiEndpoints.Value.CulturalApi);
        }

        public string GetEndpoint()
        {
            return _apiEndpoints.Value.CulturalApi;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string type)
        {
            var response = await Client.GetAsync(type);
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();

            var allLocations = JsonConvert.DeserializeObject<IEnumerable<T>>(stringContent);
            return allLocations;
        }

        public async Task<T> GetAsync<T>(long id, string type)
        {
            var response = await Client.GetAsync(type + "/" + id);
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        public dynamic CreateDataLocation(LocationViewModel location)
        {
            dynamic sendData = new ExpandoObject();
            sendData.Name = location.Name;
            sendData.Description = location.Description;
            sendData.Latitude = location.Latitude;
            sendData.Longitude = location.Longitude;
            sendData.PhotoName = location.PhotoName;

            return sendData;
        }

        public dynamic CreateDataArtist(ArtistViewModel artist)
        {
            dynamic sendData = new ExpandoObject();
            sendData.Name = artist.Name;
            sendData.Description = artist.Description;
            sendData.TypeID = artist.TypeID;
            sendData.LocationID = artist.LocationID;
            sendData.BirthDate = artist.BirthDate;
            sendData.DeathDate = artist.DeathDate;
            sendData.PhotoName = artist.PhotoName;

            return sendData;
        }

        public async Task<bool> CreateAsync<T>(T obj, string type)
        {
            ExpandoObject sendData = new ExpandoObject();
            if(type == "locations") {
                sendData = CreateDataLocation(obj as LocationViewModel);
            }
            if(type == "artists") {
                sendData = CreateDataArtist(obj as ArtistViewModel);
            }
            var jsonData = JsonConvert.SerializeObject(sendData);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(type, data);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id, string type)
        {
            var response = await Client.DeleteAsync(type + "/" + id);
            return response.IsSuccessStatusCode;
        }

        public async Task Update<T>(T obj, long id, string type)
        {
            ExpandoObject sendData = new ExpandoObject();
            if(type == "locations") {
                sendData = CreateDataLocation(obj as LocationViewModel);
            }
            if(type == "artists") {
                sendData = CreateDataArtist(obj as ArtistViewModel);
            }
            var jsonData = JsonConvert.SerializeObject(sendData);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PutAsync(type + "/" + id, data);

        }
    }
}