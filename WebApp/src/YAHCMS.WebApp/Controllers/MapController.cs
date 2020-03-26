
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class MapController : Controller
    {

        private CulturalClient _culturalClient;
        public MapController(CulturalClient culturalClient)
        {
            _culturalClient = culturalClient;
        }

        public string Route = "locations";

        public IActionResult Show()
        {
            @ViewData["endpoint"] = _culturalClient.GetEndpoint(); 
            //var locations = _culturalClient.GetAllAsync<LocationViewModel>(Route);

            return View("../Location/Map");
        }
    }
}