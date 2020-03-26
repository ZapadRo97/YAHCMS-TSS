
using YAHCMS.UserService.Models;
using Xunit;
using Moq;
using YAHCMS.UserService.Persistence;
using YAHCMS.UserService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace YAHCMS.UserService.Tests
{
    public class UsersControllerTest
    {
        [Fact]
        public void GetValidUserReturnsUser()
        {
            //arrange
            var uid = "test";
            var u = new User();
            u.UID = uid;
            u.Name = "Marcelino";
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Get(It.IsAny<string>()))
            .Returns(u);
            var controller = new UsersController(mockRepo.Object);
            //act
            var response = controller.Get(uid);
            //assert
            var viewResult = Assert.IsType<ActionResult<User>>(response);
            var model = Assert.IsAssignableFrom<User>(
                (viewResult.Result as ObjectResult).Value);

        }

        [Fact]
        public void GetNonExistentUserReturnsNotFound()
        {
            //arrange
            var mockRepo = new Mock<IUserRepository>();
            var controller = new UsersController(mockRepo.Object);
            //act
            var result = controller.Get("abc");
            //assert
            Assert.True(result.Result is NotFoundResult);

        }

        public List<User> CreateUsersList()
        {
            var user1 = new User();
            user1.UID = "abc";
            user1.Score = 1;
            var user2 = new User();
            user2.UID = "bcd";
            user2.Score = 2;

            var list = new List<User>();
            list.Add(user1);
            list.Add(user2);
            return list;
        }

        [Fact]
        public void GetAllUsersReturnsCorrectList()
        {
            //arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetAll())
            .Returns(CreateUsersList());
            var controller = new UsersController(mockRepo.Object);
            //act
            var result = controller.GetAll();
            //assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(
                (viewResult.Result as ObjectResult).Value);
            Assert.Equal(2, model.Count());

        }

         [Fact]
        public void CreateUserReturnsUser()
        {
            //A -arrange
            var user1 = new User();
            user1.UID = "abc";
            user1.Name = "Marcelino";
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Add(It.IsAny<User>()))
            .Returns(user1)
            .Verifiable();
            var controller = new UsersController(mockRepo.Object);
            //A -act
            var result = controller.Create(user1);
            //A -assert
            var viewResult = Assert.IsType<User>((result as ObjectResult).Value as User);
            Assert.Equal(viewResult.Name, "Marcelino");
            Assert.Equal((result as ObjectResult).StatusCode, 201);
        }

    }
}