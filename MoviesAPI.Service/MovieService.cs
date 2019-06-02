using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MoviesAPI.Domain;
using MoviesAPI.Repository;

namespace MoviesAPI.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public virtual async Task<IEnumerable<MovieModel>> GetAsync()
        {
            var videos = await _movieRepository.GetAsync();

            return videos;
        }

        public async Task<IEnumerable<MovieModel>> GetByCategoryAsync(string categoryTitle)
        {
            return await _movieRepository.GetByCategoryAsync(categoryTitle);
        }
    }
}
