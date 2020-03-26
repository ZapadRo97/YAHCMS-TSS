

using System.Collections.Generic;

namespace YAHCMS.CulturalService.Models
{

    public class ArtistType: BaseEntity
    {
        public string Name {get; set;}

        public string Description {get; set;}

        public List<Artist> Artists {get; set;}
    }

}