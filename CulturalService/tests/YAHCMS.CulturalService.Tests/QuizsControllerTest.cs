using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YAHCMS.CulturalService.Controllers;
using YAHCMS.CulturalService.Models;
using YAHCMS.CulturalService.Persistence;

namespace YAHCMS.CulturalService.Tests
{
    public class QuizsControllerTest
    {

        public List<Quiz> GetTestQuizs()
        {

            var quiz1 = new Quiz();
            quiz1.ID = 1;
            var quiz2 = new Quiz();
            quiz2.ID = 2;

            var list = new List<Quiz>();
            list.Add(quiz1);
            list.Add(quiz2);
            return list;
        }


        public List<QuizQuestion> GetTestQuestionQuiz()
        {

            var question1 = new QuizQuestion();
            question1.ID = 1;
            var question2 = new QuizQuestion();
            question2.ID = 2;

            var list = new List<QuizQuestion>();
            list.Add(question1);
            list.Add(question2);
            return list;
        }

        [Fact]
        public async void GetAllQuizsReturnsList()
        {
            //A -arrange
            var mockRepo = new Mock<IQuizRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync())
            .Returns(Task.FromResult(GetTestQuizs()));
            var controller = new QuizsController(mockRepo.Object);
            //A -act
            var result = await controller.GetAll();
            //A -assert
            var viewResult = Assert.IsType<ActionResult<ICollection<Quiz>>>(result);
            var model = Assert.IsAssignableFrom<ICollection<Quiz>>(
                (viewResult.Result as ObjectResult).Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async void GetAllQuestionQuizReturnsList()
        {
            var question1 = new QuizQuestion();
            question1.ID = 1;
            //A -arrange
            var mockRepo = new Mock<IQuizRepository>();
            mockRepo.Setup(repo => repo.GetQuizQuestions(It.IsAny<long>()))
            .Returns(Task.FromResult(GetTestQuestionQuiz()));
            var controller = new QuizsController(mockRepo.Object);
            //A -act
            var result = await controller.GetAllQuestions(question1.ID);
            //A -assert
            var viewResult = Assert.IsType<ActionResult<ICollection<QuizQuestion>>>(result);
            var model = Assert.IsAssignableFrom<ICollection<QuizQuestion>>(
                (viewResult.Result as ObjectResult).Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async void GetOneQuizReturnsQuiz()
        {
            //A -arrange
            var quiz1 = new Quiz();
            quiz1.ID = 1;
            quiz1.Title = "Cici";
            var mockRepo = new Mock<IQuizRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(quiz1));
            var controller = new QuizsController(mockRepo.Object);
            //A -act
            var result = await controller.GetOne(quiz1.ID);
            //A -assert
            var viewResult = Assert.IsType<Quiz>((result.Result as ObjectResult).Value);
            Assert.Equal(viewResult.Title, "Cici");
        }

        [Fact]
        public async void CreateQuizReturnsQuiz()
        {
            //A -arrange
            var quiz1 = new Quiz();
            quiz1.ID = 1;
            quiz1.Title = "Cici";
            var mockRepo = new Mock<IQuizRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Quiz>()))
            .Returns(Task.FromResult(quiz1))
            .Verifiable();
            var controller = new QuizsController(mockRepo.Object);
            //A -act
            var result = await controller.Create(quiz1);
            //A -assert
            var viewResult = Assert.IsType<Quiz>((result as ObjectResult).Value as Quiz);
            Assert.Equal(viewResult.Title, "Cici");
            Assert.Equal((result as ObjectResult).StatusCode, 201);
        }

        [Fact]
        public async void DeleteExistingArtistReturnNoContent()
        {
            //A -arrange
            var quiz1 = new Quiz();
            quiz1.ID = 1;
            var questions = new List<QuizQuestion>();
            var question1 = new QuizQuestion();
            question1.ID = 1;
            var mockRepo = new Mock<IQuizRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(quiz1));
            mockRepo.Setup(repo => repo.GetQuizQuestions(It.IsAny<long>()))
            .Returns(Task.FromResult(questions));
            mockRepo.Setup(repo => repo.DeleteQuestion(It.IsAny<QuizQuestion>())).Verifiable();
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Quiz>()))
            .Verifiable();
            var controller = new QuizsController(mockRepo.Object);
            //A -act
            var result = await controller.Delete(quiz1.ID);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 204);
        }
    }
}