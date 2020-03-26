
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.CulturalService.Models
{
    public class Quiz : BaseEntity
    {
        
        public string Title {get; set;}

        public string Description {get; set;}
        public ICollection<QuizQuestion> Questions {get; set;}
    }
}