
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class BlogController : Controller
    {

        private BlogAndPostClient _blogAndPostClient;
        private AuthManagementClient _authManagementClient;
        private CulturalClient _culturalClient;
        public IWebHostEnvironment _hostingEnvironment;

        private readonly IS3Service _s3Client;
        
        public BlogController(BlogAndPostClient blogAndPostClient,
            AuthManagementClient authManagementClient, CulturalClient culturalClient,
            IS3Service s3Client, IWebHostEnvironment hostingEnvironment)
        {
            _blogAndPostClient = blogAndPostClient;
            _authManagementClient = authManagementClient;
            _culturalClient = culturalClient;
            _s3Client = s3Client;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View("ComingSoon");
        }

        public async Task<IActionResult> ListForLocation(long id)
        {
            var blogs = await GetAllBlogs();
            foreach(var blog in blogs)
            {
                var posts = await _blogAndPostClient.GetBlogPostsAsync(blog.ID);
                if(posts != null)
                    blog.Posts = posts.Where(p => p.LocationID == id).ToList();
            }


            blogs = blogs.Where( b => b.Posts != null && (b.Posts.Count()) != 0).ToList();

            return View(blogs);
        }

        public async Task<List<BlogViewModel>> GetAllBlogs()
        {
            var allUsersList = await _authManagementClient.GetUsersIds();
            var blogs = new List<BlogViewModel>();

            foreach(var userId in allUsersList)
            {
                var userDetails = await _authManagementClient.GetUserInfo(userId);
                var userBlogs = await _blogAndPostClient.GetUserBlogs(userId);
                
                foreach(var blog in userBlogs)
                {
                    blog.userName = userDetails.name;
                }
                blogs.AddRange(userBlogs);
            }

            return blogs;
        }

        public async Task<IActionResult> AllAsync()
        {
            
            var blogs = await GetAllBlogs();

            return View();
        }

        #region Blog
        [HttpGet]
        public async Task<IActionResult> GetBlogAsync(int id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            if(User.Identity.IsAuthenticated)
            {
                ViewData["account_type"] = IdentityTools.GetAccountType(accessToken);
            }
            else
            {
                ViewData["account_type"] = "none";
            }
            
            var blog = await _blogAndPostClient.GetBlogAsync(id);
            var posts = await _blogAndPostClient.GetBlogPostsAsync(id);
            if(posts == null)
                blog.Posts = new List<PostViewModel>();
            else
                blog.Posts = posts;

            return PartialView("Show", blog);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewAsync(long id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            if(!User.Identity.IsAuthenticated)
                return this.RedirectToAction("Login", "Account");
            else {
                ViewData["account_type"] = IdentityTools.GetAccountType(accessToken);
                ViewData["view"] = "blog";
                ViewData["blog_id"] = id;
            }

            var blog = await _blogAndPostClient.GetBlogAsync(id);
            var posts = await _blogAndPostClient.GetBlogPostsAsync(id);
            if(posts == null)
                blog.Posts = new List<PostViewModel>();
            else
                blog.Posts = posts;

            return View(blog);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {   
            if(!User.Identity.IsAuthenticated)
                return this.RedirectToAction("Login", "Account");
            var model = new BlogViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] BlogViewModel newBlog)
        {
            var currentBlog = newBlog;
            currentBlog.Updated = DateTime.Now;
            currentBlog.Published = DateTime.Now;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            bool result = await _blogAndPostClient.CreateBlogAsync(newBlog, userId);

            
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            BlogViewModel blog = await _blogAndPostClient.GetBlogAsync(id);
            var posts = await _blogAndPostClient.GetBlogPostsAsync(id);
            if(posts == null)
                blog.Posts = new List<PostViewModel>();
            else
                blog.Posts = posts;
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _blogAndPostClient.DeleteBlogAsync(id);
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditAsync(long id)
        {
            BlogViewModel b = await _blogAndPostClient.GetBlogAsync(id);
            return View(b);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(long id, [FromForm] BlogViewModel blog)
        {
             if (ModelState.IsValid)
             {
                 string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                 await _blogAndPostClient.EditBlogAsync(blog, id, userId);
             }

             return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Post

        [Authorize]
        public async Task<IActionResult> ViewPostAsync(long id, long blogID)
        {
            var post = await _blogAndPostClient.GetPostAsync(blogID, id);
            var completePost = new CompletePostViewModel(post);
            if(post.ArtistID != null)
            {
                completePost.artist = await _culturalClient.GetAsync<ArtistViewModel>((long)post.ArtistID, "artists");
                
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/"+completePost.artist.PhotoName);
                if(!System.IO.File.Exists(fullPath))
                {
                    await _s3Client.ReadObjectDataAsync(completePost.artist.PhotoName);
                }
                completePost.artist.PhotoName = "/uploads/" + completePost.artist.PhotoName;
            }
            else if (post.LocationID != null)
            {
                completePost.location = await _culturalClient.GetAsync<LocationViewModel>((long)post.LocationID, "locations");
            
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/"+completePost.location.PhotoName);
                if(!System.IO.File.Exists(fullPath))
                {
                    await _s3Client.ReadObjectDataAsync(completePost.location.PhotoName);
                }
                completePost.location.PhotoName = "/uploads/" + completePost.location.PhotoName;
            }

            return View(completePost);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreatePostAsync(long id)
        {
            var artists = await _culturalClient.GetAllAsync<ArtistViewModel>("artists");
            var locations = await _culturalClient.GetAllAsync<LocationViewModel>("locations");

            var model = new PostViewModel();
            model.Locations = new List<SelectListItem>();
            model.Artists = new List<SelectListItem>();
            model.BlogID = id;

            foreach(var location in locations)
            {
                model.Locations.Add(new SelectListItem { Value=location.ID.ToString(), Text=location.Name });
            }

            foreach(var artist in artists)
            {
                model.Artists.Add(new SelectListItem { Value=artist.ID.ToString(), Text=artist.Name });
            }


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePostAsync([FromForm] PostViewModel newPost)
        {

            var currentPost = newPost;
            currentPost.Updated = DateTime.Now;
            currentPost.Published = DateTime.Now;

            bool result = await _blogAndPostClient.CreatePostAsync(newPost);

            return this.RedirectToAction("View", "Blog", new {id = newPost.BlogID});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditPostAsync(long id, long blogID)
        {
            var post = await _blogAndPostClient.GetPostAsync(blogID, id);

            var artists = await _culturalClient.GetAllAsync<ArtistViewModel>("artists");
            var locations = await _culturalClient.GetAllAsync<LocationViewModel>("locations");

            post.Locations = new List<SelectListItem>();
            post.Artists = new List<SelectListItem>();

            foreach(var location in locations)
            {
                post.Locations.Add(new SelectListItem { Value=location.ID.ToString(), Text=location.Name });
            }

            foreach(var artist in artists)
            {
                post.Artists.Add(new SelectListItem { Value=artist.ID.ToString(), Text=artist.Name });
            }

            return View(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPostAsync(long id,  [FromForm] PostViewModel model)
        {
            await _blogAndPostClient.EditPostAsync(model, id);
            return this.RedirectToAction("View", "Blog", new {id = model.BlogID});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeletePostAsync(long id, long blogID)
        {
            var post = await _blogAndPostClient.GetPostAsync(blogID, id);
            return View(post);
        }

        [HttpPost, ActionName("DeletePost")]
        public async Task<IActionResult> DeletePostConfirmed(long id, long blogID)
        {

            await _blogAndPostClient.DeletePostAsync(id, blogID);
            return RedirectToAction("View", "Blog", new {id = blogID});
        }

        #endregion

        



    }
}