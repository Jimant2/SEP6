using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using SEP6.Models;
using System.Linq;

namespace SEP6Function
{
    public static class MovieShufflerFunction
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static List<Movies> _movies = new List<Movies>(); // Replace with your actual list of movies

        [FunctionName("MovieShuffleFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/shuffle")] HttpRequest req,
            ILogger log)
        {
            // Get movies from the external API (OMDb)
            var movies = await GetMoviesFromOMDb();

            // Shuffle the movies
            var shuffledMovies = movies.OrderBy(m => Guid.NewGuid()).ToList();

            // Return the shuffled movies as JSON
            return new JsonResult(shuffledMovies);
        }

        private static async Task<List<Movies>> GetMoviesFromOMDb()
        {
            string baseUrl = "http://www.omdbapi.com/";
            string apiKey = "c04f487";

            //not specifying a particular title to fetch all movies
            string url = $"{baseUrl}?apikey={apiKey}&s=*";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var omdbResult = JsonConvert.DeserializeObject<OMDBResult>(result);

            if (omdbResult != null && omdbResult.Search != null)
            {
                // Map OMDBResult to your Movie model or create new Movie instances
                var movies = omdbResult.Search.Select(omdbMovie => new Movies
                {
                    title = omdbMovie.Title,
                    year = omdbMovie.Year,
                    // Map other properties as needed
                }).ToList();

                return movies;
            }

            return new List<Movies>(); // Return an empty list if no movies are found
        }
    }
}
