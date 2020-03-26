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


    public class LocationssControllerTest
    {

        public List<Location> GetTestLocations()
        {

            var location1 = new Location();
            location1.ID = 1;
            var location2 = new Location();
            location2.ID = 2;

            var list = new List<Location>();
            list.Add(location1);
            list.Add(location2);
            return list;
        }

        [Fact]
        public async void GetAllLocationsReturnsList()
        {
            //A -arrange
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync())
            .Returns(Task.FromResult(GetTestLocations()));
            var controller = new LocationsController(mockRepo.Object);
            //A -act
            var result = await controller.GetAll();
            //A -assert
            var viewResult = Assert.IsType<ActionResult<ICollection<Location>>>(result);
            var model = Assert.IsAssignableFrom<ICollection<Location>>(
                (viewResult.Result as ObjectResult).Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async void GetOneLocationReturnsLocation()
        {
            //A -arrange
            var location1 = new Location();
            location1.ID = 1;
            location1.Name = "Cici";
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(location1));
            var controller = new LocationsController(mockRepo.Object);
            //A -act
            var result = await controller.GetOne(location1.ID);
            //A -assert
            var viewResult = Assert.IsType<Location>((result.Result as ObjectResult).Value);
            Assert.Equal(viewResult.Name, "Cici");
        }

        [Fact]
        public async void CreatelocationReturnsLocation()
        {
            //A -arrange
            var location1 = new Location();
            location1.ID = 1;
            location1.Name = "Cici";
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Location>()))
            .Returns(Task.FromResult(location1))
            .Verifiable();
            var controller = new LocationsController(mockRepo.Object);
            //A -act
            var result = await controller.Create(location1);
            //A -assert
            var viewResult = Assert.IsType<Location>((result as ObjectResult).Value as Location);
            Assert.Equal(viewResult.Name, "Cici");
            Assert.Equal((result as ObjectResult).StatusCode, 201);
        }

        [Fact]
        public async void DeleteExistingLocationReturnNoContent()
        {
            //A -arrange
            var location1 = new Location();
            location1.ID = 1;
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(location1));
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Location>()))
            .Verifiable();
            var controller = new LocationsController(mockRepo.Object);
            //A -act
            var result = await controller.Delete(location1.ID);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 204);
        }

        [Fact]
        public async void DeleteNonExistingLocationReturnNotFound()
        {
            //A -arrange
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()));
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Location>()))
            .Verifiable();
            var controller = new LocationsController(mockRepo.Object);
            //A -act
            var result = await controller.Delete(69);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 404);
            // TODO
            // mockRepo.Verify();
        }

        [Fact]
        public async void UpdateLocationRetunsModifiedLocation()
        {
            //A -arrange
            var location1 = new Location();
            location1.ID = 1;
            location1.Name = "Cici";
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<Location>()))
            .Verifiable();
            var controller = new LocationsController(mockRepo.Object);
            //A -act
            var result = await controller.Update(location1, 1);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 200);
        }

    }

}