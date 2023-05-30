using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using SEP6.Models;
using System.Linq;
using SEP6.Data;

namespace SEP6Function
{
    public static class MovieShufflerFunction
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static List<Movies> _movies = new List<Movies>(); // Replace with your actual list of movies

        [FunctionName("MovieShuffleFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/shuffle")] HttpRequest req)
        {
            // Get movies from the external API (OMDb)
            _movies = await GetMovies();

            // Shuffle the movies
            var shuffledMovies = _movies.OrderBy(m => Guid.NewGuid()).ToList();

            // Return the shuffled movies as JSON
            return new JsonResult(shuffledMovies);
        }

        public static async Task<List<Movies>> GetMovies()
        {
            string baseUrl = "https://app-backend-sep-230516174355.azurewebsites.net/v2/movies";
            string apiKey = "c04f487";

            Random random = new Random();
            char randomChar = (char)('a' + random.Next(26));

            // Not specifying a particular title to fetch all movies
            string url = $"{baseUrl}?title={randomChar}&{apiKey}&size=10";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var movieResult = JsonConvert.DeserializeObject<List<Movies>>(result);

            if (movieResult != null)
            {

                // Shuffle the movies by year
                var shuffledMovies = movieResult.OrderBy(m => Guid.NewGuid()).ToList();

                return shuffledMovies;
            }

            return new List<Movies>(); // Return an empty list if no movies are found
        }
    }
}
