using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Threading.Tasks;
using MoviesAPI.Repository;
using MoviesAPI.Controllers;
using MoviesAPI.Domain;
using MoviesAPI.Service;
using AutoMapper;
using MoviesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace MoviesAPI.Test.Controllers
{
    public class MoviesControllerFixture : IDisposable//Fixture class aims to share one instance of objects created for all tests
    {
        public MoviesController _controller { get; private set; }
        public Mock<IMovieService> _serviceMock { get; private set; }
        public Mock<ApplicationSettings> _appSettingsMock { get; private set; }
        public Mock<IMapper> _mapperMock { get; private set; }

        //Constructor
        public MoviesControllerFixture()
        {
            // Do "global" initialization here; Only called once.

            /// 1. Arrange
            /// **********
            //Moq Service
            _serviceMock = new Mock<IMovieService>();
            //Moq Mapper
            _mapperMock = new Mock<IMapper>();
            //Moq app settings
            _appSettingsMock = new Mock<ApplicationSettings>();
            //controller
            _controller = new MoviesController(_serviceMock.Object, _appSettingsMock.Object, _mapperMock.Object);
        }

        public void Dispose()
        {
            // Do "global" teardown here; Only called once.
        }
    }

    public class MoviesControllerTest : IClassFixture<MoviesControllerFixture>
    {
        private readonly MoviesControllerFixture _fixture;

        //Constructor
        public MoviesControllerTest(MoviesControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Index_ReturnsAnActionResult_WithAListOfMovies()
        {
            //Arrange
            _fixture._serviceMock.Setup(x => x.GetAsync()).ReturnsAsync(GetTestMovies());

            //[DONT Use] This throws error in IMapper Map fn and return empty list //https://stackoverflow.com/questions/53190867/imapper-mock-returning-null
            //_fixture._mapperMock.Setup(m => m.Map<IEnumerable<MovieModel>, IEnumerable<MovieViewModel>>(It.IsAny<IEnumerable<MovieModel>>())).Returns(GetTestMoviesViewModel()); // mapping data
            //[Use below technique] This fixed IMapper Mocking
            _fixture._mapperMock.Setup(m => m.Map<IEnumerable<MovieViewModel>>(It.IsAny<object>())).Returns(GetTestMoviesViewModel()); // mapping data

            //Act
            var result = await _fixture._controller.GetAsync();

            //Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<MovieViewModel>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(
                viewResult.Result);
            Assert.Equal(2, (model.Value as IEnumerable<MovieViewModel>).Count());
        }

        //Moq
        public static IEnumerable<MovieModel> GetTestMovies()
        {
            var movies = new List<MovieModel>();
            movies.Add(new MovieModel()
            {
                Title = "Movie 1",
                Id = Guid.NewGuid(),
                Category = new CategoryModel() { Title = "Historical", Id = Guid.NewGuid(), AddedDate = DateTime.Now },
                ImageUrl = "https://blobs.azurewebsites.net/movie1"
            });
            movies.Add(new MovieModel()
            {
                Title = "Movie 2",
                Id = Guid.NewGuid(),
                Category = new CategoryModel() { Title = "Social", Id = Guid.NewGuid(), AddedDate = DateTime.Now },
                ImageUrl = "https://blobs.azurewebsites.net/movie2"
            });
            return movies;
        }

        public static IEnumerable<MovieViewModel> GetTestMoviesViewModel()
        {
            var movies = new List<MovieViewModel>();
            movies.Add(new MovieViewModel()
            {
                Title = "Movie 1",
                Category = "Historical",
                ImageUrl = "https://blobs.azurewebsites.net/movie1"
            });
            movies.Add(new MovieViewModel()
            {
                Title = "Movie 2",
                Category = "Social",
                ImageUrl = "https://blobs.azurewebsites.net/movie2"
            });
            return movies;
        }
    }
}
