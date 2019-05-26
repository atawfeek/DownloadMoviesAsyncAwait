using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DownloadMoviesAsyncAwait.Models;
using System.Net.Http;

namespace DownloadMoviesAsyncAwait.Controllers
{
    public class HomeController : Controller
    {
        string[] sources =
        {
            "https://localhost:44383/api/movies/category/Horror",
            "https://localhost:44383/api/movies/category/Social",
            "https://localhost:44383/api/movies/category/Historical"
        };

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var sw = Stopwatch.StartNew();
            var data = GetMovies();
            sw.Stop();
            ViewBag.Elapsed = sw.ElapsedMilliseconds;
            return View(data);
        }

        public async Task<IActionResult> Async() //Async action method
        {
            var sw = Stopwatch.StartNew();
            var data = await GetMoviesAsync();
            sw.Stop();
            ViewBag.Elapsed = sw.ElapsedMilliseconds;
            return View(data);
        }

        IEnumerable<IEnumerable<Movie>> GetMovies()
        {
            var allMovies = new List<IEnumerable<Movie>>();

            //Using TPL concumes shorter time than foreach because you execute loop in parallel using threads consuming Cors
            Parallel.ForEach<string>(sources, x =>
            {
                allMovies.Add(DownloadData(x));
            });
            /*
            foreach (var url in sources)
            {
                allMovies.Add(DownloadData(url));
            }
            */

            return allMovies;
        }

        /// <summary>
        /// Async version of Get Movies
        /// </summary>
        /// <returns></returns>
        async Task<IEnumerable<IEnumerable<Movie>>> GetMoviesAsync()
        {
            var allMovieTasks = new List<Task<IEnumerable<Movie>>>();

            //Using TPL concumes shorter time than foreach because you execute loop in parallel using threads consuming Cors
            Parallel.ForEach<string>(sources, x =>
            {
                allMovieTasks.Add(DownloadDataAsync(x));
            });
            /*
            foreach (var url in sources)
            {
                allMovies.Add(DownloadData(url));
            }
            */

            var allMovies = await Task.WhenAll(allMovieTasks);

            return allMovies;
        }

        /// <summary>
        /// Sync version of the Download
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private IEnumerable<Movie> DownloadData(string url)
        {
            //best practice now is to use HttpClientFactory to centralize HttpClient instances
            //HttpClient Problems:
            /*
             * What problem does the HttpClientFactory solve?
               Before the introduction of the HttpClientFactory in .NET Core 2.1, 
               it was common to use the HttpClient to make HTTP requests to services.  
               One of the big problems with using the HttpClient was the misuse of it. 
               HttpClient implements IDisposable, when anything implements IDisposable, 
               best practice tells us that we should wrap the calls we are making inside a using statement to allow 
               proper disposal of the object. However the HTTPClient is different, even though it implements IDisposable, 
               we shouldn’t be wrapping this in a using statement. The HttpClient is reusable and thread safe, 
               so it makes it very inefficient and unnecessary to dispose of the HttpClient after each request is made. 
               When you dispose of the HttpClient object the underlying socket is not immediately released. 
               This can cause some serious issues like ‘sockets exhaustion’. 
               The recommended way is to instantiated it once and reused it throughout the life of an application.
               Unfortunately, not disposing of our HttpClient instance does not fix all of the issues with the HttpClient. 
               The issue with creating a single instance of the HttpClient is that it won’t respect DNS changes, 
               because we are creating a single instance of the HttpClient, 
               we are keeping the connection open, ready to be reused. 
               Due to these issues, the HttpClientFactory was created.
             */
            //var httpClient = new HttpClient();
            var client = _httpClientFactory.CreateClient("MoviesAPIClient");
            var httpResponseMessage = client.GetAsync(url).Result;//httpClient.GetAsync(url).Result;
            httpResponseMessage.EnsureSuccessStatusCode();
            return httpResponseMessage.Content.ReadAsAsync<IEnumerable<Movie>>().Result; //Result is block
            //When it's blocked, it returns HttpResponseMessage.
        }

        /// <summary>
        /// Async version of the Download
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        async Task<IEnumerable<Movie>> DownloadDataAsync(string url) //Important naming convension to end up with: Async
        {
            //best practice now is to use HttpClientFactory to centralize HttpClient instances
            //HttpClient Problems:
            /*
             * What problem does the HttpClientFactory solve?
               Before the introduction of the HttpClientFactory in .NET Core 2.1, 
               it was common to use the HttpClient to make HTTP requests to services.  
               One of the big problems with using the HttpClient was the misuse of it. 
               HttpClient implements IDisposable, when anything implements IDisposable, 
               best practice tells us that we should wrap the calls we are making inside a using statement to allow 
               proper disposal of the object. However the HTTPClient is different, even though it implements IDisposable, 
               we shouldn’t be wrapping this in a using statement. The HttpClient is reusable and thread safe, 
               so it makes it very inefficient and unnecessary to dispose of the HttpClient after each request is made. 
               When you dispose of the HttpClient object the underlying socket is not immediately released. 
               This can cause some serious issues like ‘sockets exhaustion’. 
               The recommended way is to instantiated it once and reused it throughout the life of an application.
               Unfortunately, not disposing of our HttpClient instance does not fix all of the issues with the HttpClient. 
               The issue with creating a single instance of the HttpClient is that it won’t respect DNS changes, 
               because we are creating a single instance of the HttpClient, 
               we are keeping the connection open, ready to be reused. 
               Due to these issues, the HttpClientFactory was created.
             */
            //var httpClient = new HttpClient();
            var client = _httpClientFactory.CreateClient("MoviesAPIClient");
            var httpResponseMessage = await client.GetAsync(url); //GetAsync is awaitable so await it!
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsAsync<IEnumerable<Movie>>(); //In Async, remove blocks (.Result)
        }

        /// <summary>
        /// Tip: always return Task if you have nothing to return in the function.
        /// Dont use void: void is only to support events to be async
        /// </summary>
        /// <returns></returns>

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
