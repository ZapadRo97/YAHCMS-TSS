
using System.Collections.Generic;

namespace YAHCMS.WebApp.ViewModels
{

    public class QuizQuestion
    {

        public QuizQuestion()
        {

        }

        public QuizQuestion(long ID)
        {
            this.ID = ID;
        }
        public long ID {get; set;}
        public string Question {get; set;}
        public string Answers {get; set;}

        public string CorrectAnswer{get; set;}
    }
    public class QuizViewModel
    {
        public long ID {get; set;}
        public string Title {get; set;}

        public string Description {get; set;}
        public ICollection<QuizQuestion> Questions {get; set;}
    }
}