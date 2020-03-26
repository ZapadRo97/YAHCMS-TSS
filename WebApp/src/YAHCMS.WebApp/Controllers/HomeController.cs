using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using YAHCMS.WebApp.Models;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BlogAndPostClient _blogAndPostClient;


        public HomeController(ILogger<HomeController> logger,
         BlogAndPostClient blogAndPostClient)
        {
            _logger = logger;
            _blogAndPostClient = blogAndPostClient;
        }

        public async Task<IActionResult> IndexAsync()
        {

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            
            if(User.Identity.IsAuthenticated && accessToken != null)
            {
                ViewData["account_type"] = IdentityTools.GetAccountType(accessToken);
                ViewData["view"] = "home";
            }
            else
            {
                ViewData["account_type"] = "none";
            }
            

            /*
            var usersApiUrl = _apiEndpoints.Value.UserApi;
            var client = new RestClient(usersApiUrl + "user/" + userId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + accessToken);
            IRestResponse response = client.Execute(request);
            */


            if((string)ViewData["account_type"] == "administrator")
                return View("AdminIndex");
            else if((string)ViewData["account_type"] == "creator")
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var blogs = await _blogAndPostClient.GetUserBlogs(userId);
                foreach(var blog in blogs)
                {
                    var posts = await _blogAndPostClient.GetBlogPostsAsync(blog.ID);
                    if(posts == null)
                        blog.Posts = new List<PostViewModel>();
                    else
                        blog.Posts = posts;
                }
                return View("CreatorIndex", blogs);
            }
            else
            {
                
                var ids = await _blogAndPostClient.GetRandomIdsAsync(10);
                return View(ids);
            }
                
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
