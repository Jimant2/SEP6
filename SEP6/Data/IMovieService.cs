using SEP6.Models;

namespace SEP6.Data
{
    public interface IMovieService
    {

        Task<List<Movies>> IndexMovie(string title);
        Task<OMDBResult> GetMoviesFromOMDb(string title);
        Task<User> GetFavoriteMovies(string username);
        Task<User> PostFavoriteMovies(string username, int movieId);

        Task<User> RemoveFromFavorites(string username, int movieId);
    }
}
