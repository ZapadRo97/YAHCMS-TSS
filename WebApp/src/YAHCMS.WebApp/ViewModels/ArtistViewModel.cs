
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YAHCMS.WebApp.ViewModels
{

    public class ArtistTypeViewModel
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
    }
    public class ArtistViewModel
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}

        public long TypeID {get; set;}
        public ArtistTypeViewModel Type {get; set;}

        public List<SelectListItem> Types {get; set;}

        public LocationViewModel Location {get; set;}

        public long LocationID {get; set;}

        public List<SelectListItem> Locations {get; set;}

        [DataType(DataType.Date)]
        public DateTime BirthDate {get; set;}
        [DataType(DataType.Date)]
        public DateTime? DeathDate {get; set;}

        public bool Alive {get; set;}
        public string PhotoName {get; set;}

        public IFormFile formFile { get; set; }
    }
}