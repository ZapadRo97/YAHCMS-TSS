
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.CulturalService.Models
{
    public class Location : BaseEntity
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public double Latitude {get; set;}
        public double Longitude {get; set;}
        public string PhotoName {get; set;}

        public List<Artist> Artists {get; set;}
    }
}