using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using YAHCMS.WebApp.Extra;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class LocationController : Controller
    {

        private CulturalClient _culturalClient;
        private IS3Service _s3Client;
        public IWebHostEnvironment _hostingEnvironment;

        public string Route = "locations";

        public LocationController(CulturalClient culturalClient, 
        IWebHostEnvironment hostingEnvironment, IS3Service s3Client)
        {
            _culturalClient = culturalClient;
            _hostingEnvironment = hostingEnvironment;
            _s3Client = s3Client;
        }

        // GET: LocationGenerated
        public async Task<IActionResult> Index()
        {

            var locations = await _culturalClient.GetAllAsync<LocationViewModel>(Route);
            return View(locations);
        }

        // GET: LocationGenerated/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LocationViewModel location = await _culturalClient.GetAsync<LocationViewModel>(id ?? 0, Route);
            if(location.PhotoName != null) 
            {
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/"+location.PhotoName);
                if(!System.IO.File.Exists(fullPath))
                {
                    await _s3Client.ReadObjectDataAsync(location.PhotoName);
                }
                location.PhotoName = "/uploads/" + location.PhotoName;
            }
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: LocationGenerated/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationGenerated/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] LocationViewModel location)
        {
            if (ModelState.IsValid)
            {
                
                //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                FileUploadHelper objFile = new FileUploadHelper();
                string fileName = objFile.GetFileName(location.formFile, true);
                await _s3Client.UploadFileAsync(location.formFile.OpenReadStream(), fileName);
                location.PhotoName = fileName;
                await _culturalClient.CreateAsync<LocationViewModel>(location, Route);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: LocationGenerated/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LocationViewModel location = await _culturalClient.GetAsync<LocationViewModel>(id ?? 0, Route);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: LocationGenerated/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [FromForm] LocationViewModel location)
        {
            
            if (id != location.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(location.formFile != null)
                {
                    //delete old photo
                    var oldPhoto = location.PhotoName;
                    await _s3Client.DeleteObjectNonVersionedBucketAsync(oldPhoto);
                    //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    FileUploadHelper objFile = new FileUploadHelper();
                    string fileName = objFile.GetFileName(location.formFile, true);
                    await _s3Client.UploadFileAsync(location.formFile.OpenReadStream(), fileName);
                    location.PhotoName = fileName;
                }

                await _culturalClient.Update<LocationViewModel>(location, id, Route);

                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: LocationGenerated/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LocationViewModel location = await _culturalClient.GetAsync<LocationViewModel>(id ?? 0, Route);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: LocationGenerated/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var location = await _culturalClient.GetAsync<LocationViewModel>(id, Route);
            await _s3Client.DeleteObjectNonVersionedBucketAsync(location.PhotoName);
            await _culturalClient.Delete(id, Route);
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(long id)
        {
            return false;
        }
    }
}
