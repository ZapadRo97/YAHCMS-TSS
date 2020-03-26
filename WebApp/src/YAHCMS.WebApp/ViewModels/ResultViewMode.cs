
namespace YAHCMS.WebApp.ViewModels
{
    public class ResultViewModel
    {

        public ResultViewModel()
        {

        }

        public ResultViewModel(long quizID, double score)
        {
            QuizID = quizID;
            Score = score;
        }

        public string Title {get; set;}
        public string Description{get; set;}

        public long QuizID {get; set;}
        public double Score {get; set;}
    }
}