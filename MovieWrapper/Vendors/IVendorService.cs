using MovieWrapper.Models;
using System.Threading.Tasks;

namespace MovieWrapper.Vendors
{
    public interface IVendorService
    {
        Task<MovieListResult> GetShowingMovies();
        Task<MovieResult> GetMovieDetails(string movieId);
        Task<MovieSessionListResult> GetMovieSessions(string movieId);
    }
}
