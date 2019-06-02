using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Domain;

namespace MoviesAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDbContext _movieDbContext;
        IMapper _mapper;

        public MovieRepository(MoviesDbContext movieDbContext
                                , IMapper mapper)
        {
            _movieDbContext = movieDbContext;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<MovieModel>> GetAsync()
        {
            /// The way you get configurations in controller
            /// [access config methodology]
            //var BlobHostUrl = _appSettings.BlobHostUrl;

            var movies = await _movieDbContext.Movies.Include(v => v.Category).ToListAsync();// Select(v => new { v.Title, v.ImageUrl, Category = v.Category.Title });
            var mappedMovies = _mapper.Map<IEnumerable<MovieModel>>(movies);

            return mappedMovies;
        }

        public async Task<IEnumerable<MovieModel>> GetByCategoryAsync(string categoryTitle)
        {
            var movies = await _movieDbContext.Movies.Where(v => v.Category.Title == categoryTitle).Include(x => x.Category)
                                                    .ToListAsync(); //.Select(x => new { x.Id, x.Title, x.ImageUrl, category = x.Category.Title })));

            return _mapper.Map<IEnumerable<MovieModel>>(movies);
        }
    }
}
