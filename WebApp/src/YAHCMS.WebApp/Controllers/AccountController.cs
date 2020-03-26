using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using YAHCMS.WebApp.Models;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class AccountController : Controller
    {

        private AuthManagementClient _authManagementClient;
        private IConfiguration _configuration;

        public AccountController(AuthManagementClient authManagementClient, IConfiguration configuration)
        {
            _authManagementClient = authManagementClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View("ComingSoon");
        }

        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() 
            { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties()
            {
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in the 
                // **Allowed Logout URLs** settings for the client.

                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var client = new HttpClient();
            //await client.GetAsync("https://zapad.eu.auth0.com/v2/logout");
            
            var redirect =HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.PathBase + Url.Action("Index", "Home");
            
            return this.Redirect($"https://{_configuration["Auth0:Domain"]}/v2/logout?client_id={_configuration["Auth0:ClientId"]}&returnTo="+Uri.EscapeDataString(redirect));

            //return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> TestManageAsync()
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userInfoViewModel = await _authManagementClient.GetUserInfo(userId);

            var allUsersList = await _authManagementClient.GetUsersIds();

            return View();
        }

        [Authorize]
        public async Task<IActionResult> ManageAsync()
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            var jwtToken = new JwtSecurityToken(accessToken);
            var permissions = jwtToken.Claims.Where(c => c.Type == "permissions").Select(c => c).ToList();
            
            

            return View();
        }

        [HttpGet]
        public IActionResult FinishRegistration()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var model = new EditUserViewModel();
            model.UID = userId;
            //todo: check sth
            model.Type = "administrator";

            return View(model);
        }

        [HttpPost]
        public IActionResult FinishRegistration([FromForm] EditUserViewModel model)
        {

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}