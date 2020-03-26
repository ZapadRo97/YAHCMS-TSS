
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {

        public QuizRepository(CulturalDbContext context) : base(context) {}
        public Task<QuizQuestion> AddQuestion(QuizQuestion question, long quidID)
        {
            throw new NotImplementedException();
        }

        public QuizQuestion DeleteQuestion(QuizQuestion question)
        {
            var q = _context.Set<QuizQuestion>().Remove(question);
            _context.SaveChanges(true);
            return q.Entity;
        }

        public async Task<List<QuizQuestion>> GetQuizQuestions(long quizID)
        {
            return await _context.Set<QuizQuestion>().Where(q => q.QuizID == quizID).ToListAsync();
        }

        new public async Task<QuizQuestion> GetByIdAsync(long id)
        {
            return await _context.Set<QuizQuestion>().Include("Questions").FirstOrDefaultAsync(c => c.ID == id);
        }
    }
}