using MoviesAPI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesAPI.Domain
{
    public class CategoryModel : Model
    {
        public string Title { get; set; }
        public ICollection<MovieModel> Movies { get; set; }
    }
}
