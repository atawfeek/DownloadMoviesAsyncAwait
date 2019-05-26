using MoviesAPI.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieModel>> GetByCategoryAsync(string categoryTitle);  // note: no async here
        Task<IEnumerable<MovieModel>> GetAsync();  // note: no async here
    }
}
