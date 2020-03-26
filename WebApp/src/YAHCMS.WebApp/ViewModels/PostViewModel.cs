
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YAHCMS.WebApp.ViewModels
{
    public class PostViewModel
    {
        public long ID {get; set;}

        [Required]
        public string Title {get; set;}

        [MinLength(129)]
        [Required]
        public string Content{get; set;}

        public DateTime Published {get; set;}
        public DateTime Updated {get; set;}
        public long BlogID {get; set;}

        public long? ArtistID {get; set;}
        public long? LocationID {get; set;}

        public List<SelectListItem> Locations {get; set;}
        public List<SelectListItem> Artists {get; set;}
        
    }
}