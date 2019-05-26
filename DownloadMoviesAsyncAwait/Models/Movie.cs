using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadMoviesAsyncAwait.Models
{
    public class Movie
    {
        string _extension = ".jpg";

        public string Title { get; set; }
        private string _imageUrl;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public string Category { get; set; }
    }
}
