
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YAHCMS.WebApp.ViewModels
{
    public class BlogViewModel
    {
        public long ID {get; set;}
        public string userName {get; set;}
        [Required]
        public string Name {get; set;}
        [Required]
        public string Description {get; set;}
        public DateTime Published {get; set;}
        public DateTime Updated {get; set;}
        [Required]
        public string Language {get; set;}
        [Required]
        public string Country {get; set;}

        public List<SelectListItem> Languages { get; } = new List<SelectListItem>
         {
             new SelectListItem { Value = "ro", Text = "ro"}
         };

         public List<SelectListItem> Countries { get; } = new List<SelectListItem>
         {
             new SelectListItem { Value = "Romania", Text = "Romania"}
         };


        public IEnumerable<PostViewModel> Posts {get; set;}
    }
}