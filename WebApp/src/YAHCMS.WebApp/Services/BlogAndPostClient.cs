
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
    public class BlogAndPostClient
    {
        public HttpClient Client {get;}

        private readonly IOptions<ApiEndpoints> _apiEndpoints;

        public BlogAndPostClient(HttpClient client, 
            IOptions<ApiEndpoints> apiEndpoints)
        {
            Client = client;
            _apiEndpoints = apiEndpoints;
            client.BaseAddress = new Uri(_apiEndpoints.Value.BlogsApi);
        }

        public async Task<IEnumerable<BlogViewModel>> GetUserBlogs(string id)
        {
            var response = await Client.GetAsync("blogs/user/" + id);
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();
            var userBlogs = JsonConvert.DeserializeObject<IEnumerable<BlogViewModel>>(stringContent);
            //var result = JsonConvert.DeserializeObject<List<Post>>(stringContent);
            return userBlogs;
        }

        public async Task<IEnumerable<PostViewModel>> GetBlogPostsAsync(long id)
        {
            var response = await Client.GetAsync("blogs/" + id + "/posts");
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<IEnumerable<PostViewModel>>(stringContent);
            return posts;

        }

        //UNTESTED
        public async Task<PostViewModel> GetPostAsync(long blogID, long postID)
        {
            var response = await Client.GetAsync("blogs/" + blogID + "/posts/" + postID);
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<PostViewModel>(stringContent);
            return post;
        }

        public async Task<IEnumerable<long>> GetRandomIdsAsync(int limit)
        {
            var response = await Client.GetAsync("blogs/random/" + limit);
            var stringContent = await response.Content.ReadAsStringAsync();

            var ids = JsonConvert.DeserializeObject<IEnumerable<long>>(stringContent);

            return ids;
        }

        public async Task<BlogViewModel> GetBlogAsync(long id)
        {
            var response = await Client.GetAsync("blogs/" + id);
            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BlogViewModel>(stringContent);
        }

        public dynamic CreateBlogData(BlogViewModel model, string userID)
        {
            dynamic sendData = new ExpandoObject();
            sendData.userID = userID;
            sendData.Name = model.Name;
            sendData.Description = model.Description;
            sendData.Published = model.Published;
            sendData.Updated = model.Updated;
            sendData.Language = model.Language;
            sendData.Country = model.Country;

            return sendData;
        }

        public async Task<bool> CreateBlogAsync(BlogViewModel model, string userID)
        {
            dynamic sendData = CreateBlogData(model, userID);

            var jsonData = JsonConvert.SerializeObject(sendData);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("blogs", data);

            return response.IsSuccessStatusCode;

        }

        public dynamic CreatePostData(PostViewModel model) 
        {
            dynamic sendData = new ExpandoObject();
            sendData.Title = model.Title;
            sendData.Content = model.Content;
            sendData.Published = model.Published;
            sendData.Updated = model.Updated;
            sendData.BlogID = model.BlogID;
            sendData.ArtistID = model.ArtistID;
            sendData.LocationID = model.LocationID;

            return sendData;
        }

        public async Task<bool> CreatePostAsync(PostViewModel model)
        {
            dynamic sendData = CreatePostData(model);

            var jsonData = JsonConvert.SerializeObject(sendData);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("blogs/"+model.BlogID+"/posts", data);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditBlogAsync(BlogViewModel blog, long id, string userID)
        {
            dynamic sendData = CreateBlogData(blog, userID);
            sendData.Updated = DateTime.Now;
            var jsonData = JsonConvert.SerializeObject(sendData);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PutAsync("blogs/" + id, data);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditPostAsync(PostViewModel post, long id)
        {
            dynamic sendData = CreatePostData(post);

            var jsonData = JsonConvert.SerializeObject(sendData);
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await Client.PutAsync("blogs/"+post.BlogID+"/posts/" + id, data);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBlogAsync(long id)
        {
            var response = await Client.DeleteAsync("blogs/" + id);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePostAsync(long id, long blogID)
        {
            var response = await Client.DeleteAsync("blogs/" + blogID + "/posts/" + id);
            return response.IsSuccessStatusCode;
        }

        
    }
}