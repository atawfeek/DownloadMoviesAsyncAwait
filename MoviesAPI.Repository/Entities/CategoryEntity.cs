using MoviesAPI.Repository.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Repository.Entities
{
    public class CategoryEntity : Entity
    {
        public string Title { get; set; }
        public ICollection<MovieEntity> Movies { get; set; }
    }
}
