using System;
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.UserService.Models {
    public class User
    {
        [Required]
        [Key]
        public string UID {get; set;}
        public string Name {get; set;}
        public string Type {get; set;}
        public double Score {get; set;}
    }
}