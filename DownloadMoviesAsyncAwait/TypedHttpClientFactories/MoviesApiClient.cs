using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadMoviesAsyncAwait.TypedHttpClientFactories
{
    public class MoviesApiClient
    {
        public HttpClient Client { get; }

        public MoviesApiClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:44383/api/Movies/category/Horror");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            Client = httpClient;
        }
    }
}
