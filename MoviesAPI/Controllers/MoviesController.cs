using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviesAPI.Service;
using MoviesAPI.ViewModels;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _IMovieService;
        private readonly ApplicationSettings _appSettings;
        IMapper _mapper;

        public MoviesController(IMovieService IMovieService
                                , ApplicationSettings appSettings
                                , IMapper mapper)
        {
            _IMovieService = IMovieService;
            _appSettings = appSettings;
            _mapper = mapper;
        }

        /// <summary>
        /// Get API
        /// </summary>
        /// <returns></returns>
        [Route("category/{categoryTitle}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetByCategoryAsync(string categoryTitle)
        {
            var movies = await _IMovieService.GetByCategoryAsync(categoryTitle);

            return Ok(_mapper.Map<IEnumerable<MovieViewModel>>(movies));
        }

        /// <summary>
        /// Get API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<MovieViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<MovieViewModel>), 210)]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetAsync()
        {
            var movies = await _IMovieService.GetAsync();

            return Ok(_mapper.Map<IEnumerable<MovieViewModel>>(movies));
        }
    }
}