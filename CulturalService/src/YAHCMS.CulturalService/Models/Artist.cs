
using System;
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.CulturalService.Models
{
    public class Artist : BaseEntity
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public long TypeID {get; set;}

        public long LocationID {get; set;}

        public DateTime BirthDate {get; set;}

        public DateTime? DeathDate {get; set;}

        public string PhotoName {get; set;}

    }
}