
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.CulturalService.Models
{
    public class BaseEntity
    {
        [Required]
        public long ID {get; set;}
    }
}