using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YAHCMS.WebApp.Extra;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class ArtistController : Controller
    {

        private CulturalClient _culturalClient;
        private IS3Service _s3Client;
        public IWebHostEnvironment _hostingEnvironment;

        private BlogAndPostClient _blogAndPostClient;

        public string Route = "artists";

        public ArtistController(CulturalClient culturalClient, 
        IWebHostEnvironment hostingEnvironment, IS3Service s3Client, BlogAndPostClient blogAndPostClient)
        {
            _culturalClient = culturalClient;
            _hostingEnvironment = hostingEnvironment;
            _s3Client = s3Client;
            _blogAndPostClient = blogAndPostClient;
        }
        
        // GET: ArtistGenerated
        public async Task<IActionResult> Index()
        {

            var artists = await _culturalClient.GetAllAsync<ArtistViewModel>(Route);

            return View(artists);
        }

        // GET: ArtistGenerated/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtistViewModel artist = await _culturalClient.GetAsync<ArtistViewModel>(id ?? 0, Route);
            artist.Location = await _culturalClient.GetAsync<LocationViewModel>(artist.LocationID, "locations");
             if(artist.PhotoName != null) 
             {
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/"+artist.PhotoName);
                if(!System.IO.File.Exists(fullPath))
                {
                    await _s3Client.ReadObjectDataAsync(artist.PhotoName);
                }
                artist.PhotoName = "/uploads/" + artist.PhotoName;
            }
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        public async Task<ArtistViewModel> InitializeSelectFields(ArtistViewModel model)
        {
            var locations = await _culturalClient.GetAllAsync<LocationViewModel>("locations");
            var artistTypes = await _culturalClient.GetAllAsync<ArtistTypeViewModel>("artists/types");
            model.Locations = new List<SelectListItem>();
            model.Types = new List<SelectListItem>();
            foreach(var location in locations)
            {
                model.Locations.Add(new SelectListItem { Value=location.ID.ToString(), Text=location.Name });
            }

            foreach(var artistType in artistTypes)
            {
                model.Types.Add(new SelectListItem {Value=artistType.ID.ToString(), Text = artistType.Name});
            }

            return model;
        }

        // GET: ArtistGenerated/Create
        public async Task<IActionResult> Create()
        {
            var model = new ArtistViewModel();
            model = await InitializeSelectFields(model);


            return View(model);
        }

        // POST: ArtistGenerated/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ArtistViewModel artist)
        {
            if (ModelState.IsValid)
            {

                //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                FileUploadHelper objFile = new FileUploadHelper();
                string fileName = objFile.GetFileName(artist.formFile, true);
                await _s3Client.UploadFileAsync(artist.formFile.OpenReadStream(), fileName);
                artist.PhotoName = fileName;
                if(artist.Alive)
                    artist.DeathDate = null;
                await _culturalClient.CreateAsync<ArtistViewModel>(artist, Route);
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: ArtistGenerated/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtistViewModel artist = await _culturalClient.GetAsync<ArtistViewModel>(id ?? 0, Route);
            artist = await InitializeSelectFields(artist);
            if(artist.DeathDate == null)
                artist.Alive = true;
            
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: ArtistGenerated/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [FromForm] ArtistViewModel artist)
        {
            
            if (id != artist.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(artist.formFile != null)
                {
                    //delete old photo
                    var oldPhoto = artist.PhotoName;
                    await _s3Client.DeleteObjectNonVersionedBucketAsync(oldPhoto);
                    //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    FileUploadHelper objFile = new FileUploadHelper();
                    string fileName = objFile.GetFileName(artist.formFile, true);
                    await _s3Client.UploadFileAsync(artist.formFile.OpenReadStream(), fileName);
                    artist.PhotoName = fileName;
                }
                if(artist.Alive)
                    artist.DeathDate = null;

                await _culturalClient.Update<ArtistViewModel>(artist, id, Route);
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: ArtistGenerated/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtistViewModel artist= await _culturalClient.GetAsync<ArtistViewModel>(id ?? 0, Route);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: ArtistGenerated/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var artist = await _culturalClient.GetAsync<ArtistViewModel>(id, Route);
            await _s3Client.DeleteObjectNonVersionedBucketAsync(artist.PhotoName);
            await _culturalClient.Delete(id, Route);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> List()
        {
            var fullArtists = new Dictionary<ArtistTypeViewModel, List<ArtistWithPostViewModel>>();

            var blogIds = await _blogAndPostClient.GetRandomIdsAsync(9999);
            var blogs = new Dictionary<BlogViewModel, List<PostViewModel>>();
            foreach(var blogId in blogIds) 
            {
                blogs.Add(await _blogAndPostClient.GetBlogAsync(blogId), 
                    (await _blogAndPostClient.GetBlogPostsAsync(blogId)).ToList());
            }

            var artists = await _culturalClient.GetAllAsync<ArtistViewModel>(Route);
            var types = await _culturalClient.GetAllAsync<ArtistTypeViewModel>("artists/types");

            var typesMap = new Dictionary<long, ArtistTypeViewModel>();
            foreach(var type in types)
            {
                typesMap.Add(type.ID, type);
            }
            foreach(var artist in artists)
            {
                artist.Type = typesMap[artist.TypeID];
                if(!fullArtists.ContainsKey(typesMap[artist.TypeID])) 
                {
                    fullArtists.Add(typesMap[artist.TypeID], new List<ArtistWithPostViewModel>());
                }
                var fullArtist = new ArtistWithPostViewModel(artist);

                foreach(var blog in blogs.Keys) {

                    var validPosts = blogs[blog].Where(p => p.ArtistID == artist.ID).ToList();
                    if(validPosts.Count() != 0 )
                    {
                        if(fullArtist.posts == null)
                        {
                            fullArtist.posts = new Dictionary<BlogViewModel, List<PostViewModel>>();
                        }
                        fullArtist.posts.Add(blog, validPosts);
                    }
                }

                fullArtists[typesMap[artist.TypeID]].Add(fullArtist);
            }


            return View(fullArtists);
        }
    }
}
