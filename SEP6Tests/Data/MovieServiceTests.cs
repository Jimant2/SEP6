using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SEP6.Data;
using SEP6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SEP6.Data.Tests
{
    [TestClass]
    public class MovieServiceTests
    {

        [TestMethod]
        public async Task IndexMovieTest()
        {
            // Arrange
            var httpClient = new HttpClient();
            var movieService = new MovieService(httpClient);
            string searchedMovie = "The Matrix";

            // Act
            var result = await movieService.IndexMovie(searchedMovie);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "The list should contain movies");

            
            foreach (var movie in result)
            {
                Assert.IsNotNull(movie.title, "Movie title should not be null");
                Assert.IsNotNull(movie.year, "Movie year should not be null");
               
            }
        }


        [TestMethod]
        public async Task GetFavoriteMoviesTest()
        {
            // Arrange
            var httpClient = new HttpClient();
            var movieService = new MovieService(httpClient);
            string username = "testuser";

            // Act
            var result = await movieService.GetFavoriteMovies(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Name, "User name should match");

            
            if (result.Favorites != null)
            {
                foreach (var movie in result.Favorites)
                {
                    Assert.IsNotNull(movie.title, "Movie title should not be null");
                    Assert.IsNotNull(movie.year, "Movie year should not be null");
                    
                }
            }
        }

        [TestMethod]
        public async Task PostFavoriteMoviesTest()
        {
            // Arrange
            var httpClient = new HttpClient(); 
            var movieService = new MovieService(httpClient);
            string username = "testuser";
            int movieId = 39442; 

            // Act
            var result = await movieService.PostFavoriteMovies(username, movieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Name, "User name should match");

            
            var favoriteMovies = result.Favorites;
            Assert.IsNotNull(favoriteMovies, "Favorites list should not be null");
            Assert.IsTrue(favoriteMovies.Any(m => m.Id == movieId), "The specified movie should be added to favorites");
        }
    }
}