
using Microsoft.AspNetCore.Http;

namespace YAHCMS.WebApp.ViewModels 
{
    public class LocationViewModel
    {
        public long ID {get; set;}
        public string Name {get; set;}

        public string Description {get; set;}

        public double Latitude {get; set;}

        public double Longitude {get; set;}

        public string PhotoName { get; set; }

        public IFormFile formFile { get; set; }
    }
}