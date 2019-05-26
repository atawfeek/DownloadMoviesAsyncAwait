using MoviesAPI.Repository.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Repository.Entities
{
    public class MovieEntity : Entity
    {
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
