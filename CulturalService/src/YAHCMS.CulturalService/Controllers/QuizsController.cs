

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YAHCMS.CulturalService.Models;
using YAHCMS.CulturalService.Persistence;

namespace YAHCMS.CulturalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizsController : ControllerBase
    {
        private IQuizRepository _repository;

        public QuizsController(IQuizRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Quiz>>> GetAll()
        {
            List<Quiz> result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("questions/{id}")]
        public async Task<ActionResult<ICollection<QuizQuestion>>> GetAllQuestions(long id)
        {
            List<QuizQuestion> result = await _repository.GetQuizQuestions(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetOne(long id)
        {
            Quiz result = await _repository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Quiz quiz)
        {
            Quiz l = await _repository.CreateAsync(quiz);

            //create questions

            return Created($"{l.ID}", quiz);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            Quiz quiz = await _repository.GetByIdAsync(id);
            quiz.Questions = await _repository.GetQuizQuestions(id);
            if(quiz == null)
                return NotFound();

/*
            foreach(var question in quiz.Questions)
            {
                _repository.DeleteQuestion(question);
            }
  */
            var questions = quiz.Questions.ToList();
            for(var i = 0; i < questions.Count; i++)
            {
                _repository.DeleteQuestion(questions[i]);
            }

            _repository.Delete(quiz);

            return NoContent();
        }
    }
}