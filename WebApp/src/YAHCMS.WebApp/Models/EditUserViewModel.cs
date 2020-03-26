using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YAHCMS.WebApp.Models
{
    public class EditUserViewModel
    {
        [MinLength(5)]
        [Required]
        public string Name {get; set;}

        [Required]
        public string UID {get; set;}

        public string Type {get; set;}

        public List<SelectListItem> UserTypes {get; } = new List<SelectListItem>
        {
            new SelectListItem {Value = "administrator", Text = "Administrator"},
            new SelectListItem {Value = "creator", Text = "Creator"},
            new SelectListItem {Value = "follower", Text = "Follower"}
        };
    }
}