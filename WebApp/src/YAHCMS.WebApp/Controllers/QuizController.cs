
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YAHCMS.WebApp.Extra;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Controllers
{
    public class QuizController : Controller
    {

        public string Route = "quizs";
        private CulturalClient _culturalClient;
        private UserClient _userClient;
        public QuizController(CulturalClient culturalClient, UserClient userClient)
        {
            _culturalClient = culturalClient;
            _userClient = userClient;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var quizes = await _culturalClient.GetAllAsync<QuizViewModel>(Route);
            foreach(var quiz in quizes)
            {
                quiz.Questions = (await _culturalClient.GetAllAsync<QuizQuestion>(Route + "/questions/" + quiz.ID)).ToList();
            }
            return View(quizes);
        }

        public IActionResult Create()
        {
            ViewData["endpoint"] = _culturalClient.GetEndpoint(); 
            return View();
        }

        public IActionResult CreateQuestion(long id)
        {
            var question = new QuizQuestion(id);
            return View(question);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuizViewModel quiz = await _culturalClient.GetAsync<QuizViewModel>((long)id, Route);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _culturalClient.Delete(id, Route);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListAsync()
        {
            var quizes = await _culturalClient.GetAllAsync<QuizViewModel>(Route);
            foreach(var quiz in quizes)
            {
                quiz.Questions = (await _culturalClient.GetAllAsync<QuizQuestion>(Route + "/questions/" + quiz.ID)).ToList();
            }
            return View(quizes);
        }

        public async Task<IActionResult> SolveAsync(long id)
        {
            var quiz = await _culturalClient.GetAsync<QuizViewModel>(id, Route);
            quiz.Questions = (await _culturalClient.GetAllAsync<QuizQuestion>(Route + "/questions/" + quiz.ID)).ToList();
            return View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> Process([FromBody] SendAnswersViewModel answers)
        {
            var result = new ResultViewModel();
            result.QuizID = answers.ID;
            var userAnswers = answers.Answers.ToList();
            var ratio = 100/answers.Answers.Count;
            var questions = (await _culturalClient.GetAllAsync<QuizQuestion>(Route + "/questions/" + result.QuizID)).ToList();
            var totalScore = 0.0;
            for(var i = 0; i < questions.Count; i++) 
            {
                if(questions[i].CorrectAnswer == userAnswers[i]) {
                    totalScore += ratio;
                }
            }
            result.Score = totalScore;

            //return RedirectToAction("Result", result);
            return Ok(result);
        }

        public async Task<IActionResult> Result(long quizID, double score)
        {
            //do something with user
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //await _userClient.CreateAsync
            var userModel = new UserViewModel();
            userModel.UID = currentUserId;
            userModel.Type = "Unspecified";
            userModel.Score = score;
            userModel.Name = await ViewUtils.GetUserNameAsync(currentUserId);

            await _userClient.CreateAsync(userModel);

            var result = new ResultViewModel(quizID, score);
            var quiz = await _culturalClient.GetAsync<QuizViewModel>(result.QuizID, Route);
            result.Title = quiz.Title;
            result.Description = quiz.Description;
            return View(result);
        }


        public async Task<IActionResult> Leaderboard()
        {

            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var users = await _userClient.GetAllAsync();
            foreach(var user in users)
            {
                if(user.UID == currentUserId)
                    user.Type="current";
            }


            return View(users);
        }

        
    }
}