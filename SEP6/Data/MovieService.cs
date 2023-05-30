using Newtonsoft.Json;
using SEP6.Models;
using Syncfusion.Blazor.Inputs;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace SEP6.Data
{
    public class MovieService : IMovieService
    {
        private HttpClient httpClient;

        public Movies movies { get; set; }

        public MovieService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public MovieService()
        {
        }

        public async Task<OMDBResult> GetMoviesFromOMDb(string title)
        {
            string baseUrl = "http://www.omdbapi.com/";
            string apiKey = "c04f487";

            string encodedTitle = Uri.EscapeDataString(title);
            string url = $"{baseUrl}?apikey={apiKey}&s={encodedTitle}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();


            var result = await response.Content.ReadAsStringAsync();
            var listOfMovieResults = JsonConvert.DeserializeObject<OMDBResult>(result);

            return listOfMovieResults;
        }


        public async Task<List<Movies>> IndexMovie(string searchedMovie)
        {
            string baseUrl = "https://app-backend-sep-230516174355.azurewebsites.net/movies";
            string encodedTitle = Uri.EscapeDataString(searchedMovie);
            string url = $"{baseUrl}?title={encodedTitle}";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            List<Movies> moviesResult = JsonConvert.DeserializeObject<List<Movies>>(responseContent);

            if (moviesResult != null)
            {
                foreach (var movie in moviesResult.Where(x => !String.IsNullOrEmpty(x.title)))
                {
                    Console.WriteLine($"Processing movie: {movie.title}");
                    await UpdateMovieWithOMDbDetails(movie);
                }
            }

            return moviesResult;
        }

        public async Task<User> GetFavoriteMovies(string username)
        {
            User user = new User();
            Console.WriteLine("Entering GetFavoriteMovies");
            string baseUrl = "https://app-backend-sep-230516174355.azurewebsites.net/topList";
            string name = username;

            string encodedUsername = Uri.EscapeDataString(username);
            string url = $"{baseUrl}?username={encodedUsername}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.SendAsync(httpRequestMessage);
                var responseStatusCode = response.StatusCode.ToString().ToLower();

                if (responseStatusCode.Equals("ok"))
                {
                    user.Name = name;

                    // Retrieve the list of favorite movies
                    user.Favorites = await response.Content.ReadFromJsonAsync<List<Movies>>();

                    foreach (var movie in user.Favorites.Where(x => !String.IsNullOrEmpty(x.title)))
                    {
                        Console.WriteLine($"Processing movie: {movie.title}");
                        await UpdateMovieWithOMDbDetails(movie);
                    }
                    return user;
                }
                else
                {
                    throw new Exception(responseStatusCode);
                }
            }
        }

        private async Task UpdateMovieWithOMDbDetails(Movies movie)
        {
            var omdbResult = await GetMoviesFromOMDb(movie.title);

            if (omdbResult != null && omdbResult.Search != null)
            {
                var omdbMovieMatch = omdbResult.Search
                    .Where(x => int.Parse(x.ImdbID.Replace("t", "")) == movie.Id)
                    .FirstOrDefault();

                if (omdbMovieMatch != null)
                {
                    movie.Type = omdbMovieMatch.Type;
                    movie.Poster = omdbMovieMatch.Poster;
                }
            }
            else
            {
                Console.WriteLine($"No OMDB result or search results for movie: {movie.title}");

                movie.Type = "Unknown";
                movie.Poster = "default-poster.jpg";
            }
        }

        public async Task<User> PostFavoriteMovies(string username, int movieId)
        {
            User user = new User();
            Movies movie = new Movies();
            user.Favorites = new List<Movies>();

            Console.WriteLine("Entering PostFavoriteMovies");
            string baseUrl = "https://app-backend-sep-230516174355.azurewebsites.net/topList";
            string name = username;

            string encodedUsername = Uri.EscapeDataString(username);
            string url = $"{baseUrl}?username={encodedUsername}&movieId={movieId}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.SendAsync(httpRequestMessage);
                var responseStatusCode = response.StatusCode.ToString().ToLower();

                if (responseStatusCode.Equals("ok"))
                {
                    user.Name = name;
                    movie.Id = movieId;

                    // Add the movie to the user's favorite movies list
                    user.Favorites.Add(movie);

                    return user;
                }
                else
                {
                    throw new Exception(responseStatusCode);
                }
            }
        }

        public async Task<User> RemoveFromFavorites(string username, int movieId)
        {
            User user = new User();
            Movies movie = new Movies();
            user.Favorites = new List<Movies>();

            Console.WriteLine("Entering RemoveFavoriteMovie");
            string baseUrl = "https://app-backend-sep-230516174355.azurewebsites.net/topList";
            string name = username;

            string encodedUsername = Uri.EscapeDataString(username);
            string url = $"{baseUrl}?username={encodedUsername}&movieId={movieId}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            using (HttpClient client = new HttpClient())
            {                                                                                               
                var response = await client.SendAsync(httpRequestMessage);
                var responseStatusCode = response.StatusCode.ToString().ToLower();

                if (responseStatusCode.Equals("ok"))
                {
                    user.Name = name;
                    movie.Id = movieId;

                    // Remove the movie from the user's favorite movies list
                    user.Favorites.Remove(movie);

                    return user;
                }
                else
                {
                    throw new Exception(responseStatusCode);
                }
            }
        }
    }
}
