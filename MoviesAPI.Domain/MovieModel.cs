using MoviesAPI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesAPI.Domain
{
    public class MovieModel : Model
    {
        public string Title { get; set; }
        public CategoryModel Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
