
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.CulturalService.Models
{
    public class QuizQuestion : BaseEntity
    {
        public string Question {get; set;}
        public string Answers {get; set;}

        public string CorrectAnswer{get; set;}

        public long QuizID {get; set;}

        
    }
}