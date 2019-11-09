using MovieWrapper.Models;
using MovieWrapper.Utils;
using MovieWrapper.Vendors.GalaxyCinema;
using MovieWrapper.Vendors.Lotteria;
using System.Threading.Tasks;

namespace MovieWrapper
{
    public interface IMovieService
    {
        Task<MovieListResult> GetShowingMovies(VendorType type);
        Task<MovieListResult> GetShowingMovies(VendorType type, string domain);
        Task<MovieResult> GetMovieDetails(VendorType type, string movieId);
        Task<MovieResult> GetMovieDetails(VendorType type, string domain, string movieId);
        Task<MovieSessionListResult> GetMovieSessions(VendorType type, string movieId);
        Task<MovieSessionListResult> GetMovieSessions(VendorType type, string domain, string movieId);
    }

    public class MovieService : IMovieService
    {
        private readonly IRequester _requester;
        public MovieService()
        {
            _requester = new Requester();
        }

        #region Showing Movies
        /// <summary>
        /// Get current showing movies (default domain)
        /// </summary>
        /// <param name="type">Type of vender (ex: Galaxy, Lotte, ...)</param>
        /// <returns>List of current show movie model with status and message</returns>
        public async Task<MovieListResult> GetShowingMovies(VendorType type)
        {
            if (type == VendorType.GalaxyCinema)
            {
                var galaxyService = new GalaxyService(_requester);
                return await galaxyService.GetShowingMovies();
            }
            else if (type == VendorType.Lotteria)
            {
                var lotteService = new LotteService(_requester);
                return await lotteService.GetShowingMovies();
            }
            else
            {
                return new MovieListResult
                {
                    Success = false,
                    Message = "Not implemented"
                };
            };
        }

        /// <summary>
        /// Get current showing movies (new domain)
        /// </summary>
        /// <param name="type">Type of vender (ex: Galaxy, Lotte, ...)</param>
        /// <returns>List of current show movie model with status and message</returns>
        public async Task<MovieListResult> GetShowingMovies(VendorType type, string domain)
        {
            if (type == VendorType.GalaxyCinema)
            {
                var galaxyService = new GalaxyService(domain, _requester);
                return await galaxyService.GetShowingMovies();
            }
            else if (type == VendorType.Lotteria)
            {
                var lotteService = new LotteService(domain, _requester);
                return await lotteService.GetShowingMovies();
            }
            else
            {
                return new MovieListResult
                {
                    Success = false,
                    Message = "Not implemented"
                };
            };
        }
        #endregion

        #region Movie Details
        /// <summary>
        /// Get movie details (default domain)
        /// </summary>
        /// <param name="type">Type of vender (ex: Galaxy, Lotte, ...)</param>
        /// <param name="movieId">Moive's id, type string: different id's type between vendors (string is the best type here)</param>
        /// <returns>Detail of movie with status and message</returns>
        public async Task<MovieResult> GetMovieDetails(VendorType type, string movieId)
        {
            if (type == VendorType.GalaxyCinema)
            {
                var galaxyService = new GalaxyService(_requester);
                return await galaxyService.GetMovieDetails(movieId);
            }
            else if (type == VendorType.Lotteria)
            {
                var lotteService = new LotteService(_requester);
                return await lotteService.GetMovieDetails(movieId);
            }
            else
            {
                return new MovieResult
                {
                    Success = false,
                    Message = "Not implemented"
                };
            };
        }

        /// <summary>
        /// Get movie details (default domain)
        /// </summary>
        /// <param name="type">Type of vender (ex: Galaxy, Lotte, ...)</param>
        /// <param name="movieId">Moive's id, type string: different id's type between vendors (string is the best type here)</param>
        /// <returns>Detail of movie with status and message</returns>
        public async Task<MovieResult> GetMovieDetails(VendorType type, string domain, string movieId)
        {
            if (type == VendorType.GalaxyCinema)
            {
                var galaxyService = new GalaxyService(domain, _requester);
                return await galaxyService.GetMovieDetails(movieId);
            }
            else if (type == VendorType.Lotteria)
            {
                var lotteService = new LotteService(domain, _requester);
                return await lotteService.GetMovieDetails(movieId);
            }
            else
            {
                return new MovieResult
                {
                    Success = false,
                    Message = "Not implemented"
                };
            };
        }
        #endregion

        #region Movie Sessions
        /// <summary>
        /// Get movie sessions (default domain)
        /// </summary>
        /// <param name="type">Type of vender (ex: Galaxy, Lotte, ...)</param>
        /// <param name="movieId">Moive's id, type string: different id's type between vendors (string is the best type here)</param>
        /// <returns>List of movie sessions with status and message with status and message</returns>
        public async Task<MovieSessionListResult> GetMovieSessions(VendorType type, string movieId)
        {
            if (type == VendorType.GalaxyCinema)
            {
                var galaxyService = new GalaxyService(_requester);
                return await galaxyService.GetMovieSessions(movieId);
            }
            else if (type == VendorType.Lotteria)
            {
                var lotteService = new LotteService(_requester);
                return await lotteService.GetMovieSessions(movieId);
            }
            else
            {
                return new MovieSessionListResult
                {
                    Success = false,
                    Message = "Not implemented"
                };
            };
        }

        /// <summary>
        /// Get movie sessions (new domain)
        /// </summary>
        /// <param name="type">Type of vender (ex: Galaxy, Lotte, ...)</param>
        /// <param name="movieId">Moive's id, type string: different id's type between vendors (string is the best type here)</param>
        /// <returns>List of movie sessions with status and message with status and message</returns>
        public async Task<MovieSessionListResult> GetMovieSessions(VendorType type, string domain, string movieId)
        {
            if (type == VendorType.GalaxyCinema)
            {
                var galaxyService = new GalaxyService(domain, _requester);
                return await galaxyService.GetMovieSessions(movieId);
            }
            else if (type == VendorType.Lotteria)
            {
                var lotteService = new LotteService(domain, _requester);
                return await lotteService.GetMovieSessions(movieId);
            }
            else
            {
                return new MovieSessionListResult
                {
                    Success = false,
                    Message = "Not implemented"
                };
            };
        }
        #endregion
    }
}
