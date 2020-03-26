
using System.Collections.Generic;
using System.Threading.Tasks;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public interface IQuizRepository : IBaseRepository<Quiz>
    {
        Task<List<QuizQuestion>> GetQuizQuestions(long quizID);

        Task<QuizQuestion> AddQuestion(QuizQuestion question, long quidID);

        QuizQuestion DeleteQuestion(QuizQuestion question);
    }
}