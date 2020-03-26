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
    public class ArtistsControllerTest
    {

        public List<Artist> GetTestArtists()
        {

            var artist1 = new Artist();
            artist1.ID = 1;
            var artist2 = new Artist();
            artist2.ID = 2;

            var list = new List<Artist>();
            list.Add(artist1);
            list.Add(artist2);
            return list;
        }

        public List<ArtistType> GetTestArtistsTypes()
        {

            var artist1 = new ArtistType();
            artist1.ID = 1;
            var artist2 = new ArtistType();
            artist2.ID = 2;

            var list = new List<ArtistType>();
            list.Add(artist1);
            list.Add(artist2);
            return list;
        }

        [Fact]
        public async void GetAllArtistsReturnsList()
        {
            //A -arrange
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync())
            .Returns(Task.FromResult(GetTestArtists()));
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.GetAll();
            //A -assert
            var viewResult = Assert.IsType<ActionResult<ICollection<Artist>>>(result);
            var model = Assert.IsAssignableFrom<ICollection<Artist>>(
                (viewResult.Result as ObjectResult).Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async void GetAllArtistsTypesReturnsList()
        {
            //A -arrange
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.GetArtistTypesAsync())
            .Returns(Task.FromResult(GetTestArtistsTypes()));
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.GetAllTypes();
            //A -assert
            var viewResult = Assert.IsType<ActionResult<ICollection<ArtistType>>>(result);
            var model = Assert.IsAssignableFrom<ICollection<ArtistType>>(
                (viewResult.Result as ObjectResult).Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async void GetOneArtistReturnsArtist()
        {
            //A -arrange
            var artist1 = new Artist();
            artist1.ID = 1;
            artist1.Name = "Marcelinos";
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(artist1));
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.GetOne(artist1.ID);
            //A -assert
            var viewResult = Assert.IsType<Artist>((result.Result as ObjectResult).Value);
            Assert.Equal(viewResult.Name, "Marcelinos");
        }

        [Fact]
        public async void CreateArtistReturnsArtist()
        {
            //A -arrange
            var artist1 = new Artist();
            artist1.ID = 1;
            artist1.Name = "Marcelinos";
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Artist>()))
            .Returns(Task.FromResult(artist1))
            .Verifiable();
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.Create(artist1);
            //A -assert
            var viewResult = Assert.IsType<Artist>((result as ObjectResult).Value as Artist);
            Assert.Equal(viewResult.Name, "Marcelinos");
            Assert.Equal((result as ObjectResult).StatusCode, 201);
        }

        [Fact]
        public async void DeleteExistingArtistReturnNoContent()
        {
            //A -arrange
            var artist1 = new Artist();
            artist1.ID = 1;
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(artist1));
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Artist>()))
            .Verifiable();
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.Delete(artist1.ID);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 204);
        }


        [Fact]
        public async void DeleteNonExistingArtistReturnNotFound()
        {
            //A -arrange
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()));
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Artist>()))
            .Verifiable();
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.Delete(69);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 404);
            // TODO
            // mockRepo.Verify();
        }

        [Fact]
        public async void UpdateArtistRetunsModifiedArtist()
        {
            //A -arrange
            var artist1 = new Artist();
            artist1.ID = 1;
            artist1.Name = "Cici";
            var mockRepo = new Mock<IArtistRepository>();
            mockRepo.Setup(repo => repo.Update(It.IsAny<Artist>()))
            .Verifiable();
            var controller = new ArtistsController(mockRepo.Object);
            //A -act
            var result = await controller.Update(artist1,1);
            //A -assert
            Assert.Equal((result as StatusCodeResult).StatusCode, 200);
        }

    }
}