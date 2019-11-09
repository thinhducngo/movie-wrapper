using MovieWrapper.Models;
using MovieWrapper.Utils;
using MovieWrapper.Vendors.GalaxyCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWrapper.Vendors.GalaxyCinema
{
    public class GalaxyService : IVendorService
    {
        private readonly string _domain;
        private readonly IRequester _requester;

        public GalaxyService(IRequester requester)
        {
            _domain = "https://www.galaxycine.vn";
            _requester = requester;
        }

        public GalaxyService(string domain, IRequester requester)
        {
            _domain = domain;
            _requester = requester;
        }

        /// <summary>
        /// Get current showing movies of Galaxy cinema
        /// Ref: https://www.galaxycine.vn/dat-ve/phap-su-mu-ai-chet-gio-tay
        /// </summary>
        /// <returns>List of current showing movie of Galaxy cinema with status and message</returns>
        public async Task<MovieListResult> GetShowingMovies()
        {
            // Handle request exception
            try
            {
                var response = await _requester.Get<ShowAndComingModel>($"{_domain}/api/movie/showAndComming");

                return new MovieListResult
                {
                    Success = true,
                    Data = response.MovieShowing
                        .Select(x => Mapper.MapToMovie(x))
                        .ToList()
                };
            }
            catch (Exception e)
            {
                return new MovieListResult
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        /// <summary>
        /// Get Galaxy cinema movie details
        /// Note: Galaxy does not have api for getting movie details -> get all and filter
        /// Ref: https://www.galaxycine.vn/dat-ve/phap-su-mu-ai-chet-gio-tay
        /// </summary>
        /// <param name="movieId">Galaxy movie id</param>
        /// <returns>Detail of Galaxy cinema movie with status and message</returns>
        public async Task<MovieResult> GetMovieDetails(string movieId)
        {
            // Handle request exception
            try
            {
                var response = await _requester.Get<ShowAndComingModel>($"{_domain}/api/movie/showAndComming");

                var guidId = new Guid(movieId);
                var movie = response.MovieShowing.Concat(response.MovieCommingSoon).FirstOrDefault(x => x.Id == guidId);
                if (movie == null)
                {
                    return new MovieResult
                    {
                        Success = false,
                        Message = "Not found"
                    };
                }
                else
                {
                    return new MovieResult
                    {
                        Success = true,
                        Data = Mapper.MapToMovie(movie)
                    };
                }
            }
            catch (Exception e)
            {
                return new MovieResult
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        /// <summary>
        /// Get movie sessions of Galaxy cinema
        /// Ref: https://www.galaxycine.vn/dat-ve/phap-su-mu-ai-chet-gio-tay
        /// </summary>
        /// <param name="movieId">Galaxy movie id</param>
        /// <returns>List of sessions with status and message with status and message</returns>
        public async Task<MovieSessionListResult> GetMovieSessions(string movieId)
        {
            // Handle request exception
            try
            {
                var response = await _requester.Get<List<GalaxyMovieSession>>($"{_domain}/api/session/movie/{movieId}");
                var sessionItems = SplitSessions(response);

                return new MovieSessionListResult
                {
                    Success = true,
                    Data = sessionItems
                        .Select(x => Mapper.MapToMovieSession(x, movieId))  // Galaxy does not return move id
                        .ToList()
                };
            }
            catch (Exception e)
            {
                return new MovieSessionListResult
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        /// <summary>
        /// In one session contains multiple group of date and time -> split
        /// </summary>
        /// <param name="sessions">Galaxy movie session model</param>
        /// <returns></returns>
        public IList<GalaxyMovieSessionItem> SplitSessions(List<GalaxyMovieSession> sessions)
        {
            return sessions.SelectMany(x => x.Dates
                .SelectMany(date => date.Bundles
                    .SelectMany(bundle => bundle.Sessions
                    .Select(session =>
                    {
                        return new GalaxyMovieSessionItem
                        {
                            Address = x.Address,
                            ShowDate = session.ShowDate,
                            ShowTime = session.ShowTime
                        };
                    }))))
                .ToList();
        }
    }
}
