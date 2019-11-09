using MovieWrapper.Models;
using MovieWrapper.Vendors.GalaxyCinema.Models;
using MovieWrapper.Vendors.LotteCinema.Models;
using System.Collections.Generic;

namespace MovieWrapper.Utils
{
    /// <summary>
    /// Map specify data model of vendor -> app model
    /// Note: we can use AutoMapper for mapping ()
    /// Manual mapping reason: Handle mapping exactly
    /// </summary>
    public static class Mapper
    {
        #region Galaxy
        /// <summary>
        /// Map Galaxy movie model to app moive model
        /// </summary>
        /// <param name="galaxyMovie">Galaxy movie model</param>
        /// <returns>App Movie model</returns>
        public static Movie MapToMovie(GalaxyMovie galaxyMovie)
        {
            return new Movie
            {
                Id = galaxyMovie.Id.ToString(),
                Name = galaxyMovie.Name,
                ReleaseDate = galaxyMovie.Startdate,
                Rating = galaxyMovie.Point,
                Description = galaxyMovie.Description
            };
        }

        /// <summary>
        /// Map Galaxy movie session model to app movie session model
        /// </summary>
        /// <param name="galaxyMovieSession">Galaxy movie session model separated by date</param>
        /// <param name="movieId">>Galaxy movie id</param>
        /// <returns>App Movie session model</returns>
        public static MovieSession MapToMovieSession(GalaxyMovieSessionItem galaxyMovieSession, string movieId)
        {
            return new MovieSession
            {
                MovieId = movieId,
                Location = galaxyMovieSession.Address,
                ShowDate = galaxyMovieSession.ShowDate,
                ShowTime = galaxyMovieSession.ShowTime
            };
        }
        #endregion

        #region Lotte
        /// <summary>
        /// Map Lotte movie model to app movie model
        /// </summary>
        /// <param name="lotteMovie">Lotte movie model in home page</param>
        /// <returns>App Movie model</returns>
        public static Movie MapToMovie(LotteMovieShortView lotteMovie)
        {
            return new Movie
            {
                Id = lotteMovie.RepresentationMovieCode,
                Name = lotteMovie.MovieName,
                ReleaseDate = Formatter.FormatToDateTime(lotteMovie.ReleaseDate, "yyyyMMdd"),
                Rating = lotteMovie.ViewEvaluation
            };
        }

        /// <summary>
        /// Map Lotte movie model to app movie model
        /// </summary>
        /// <param name="lotteMovie">Lotte movie detail model</param>
        /// <returns>App Movie model</returns>
        public static Movie MapToMovie(LotteMovie lotteMovie)
        {
            return new Movie
            {
                Id = lotteMovie.RepresentationMovieCode,
                Name = lotteMovie.MovieName,
                ReleaseDate = Formatter.FormatToDateTime(lotteMovie.ReleaseDate, "yyyyMMdd"),
                Rating = lotteMovie.ViewEvaluation,
                Description = lotteMovie.Synopsis
            };
        }

        /// <summary>
        /// Map Galaxy movie session model to app movie session model
        /// </summary>
        /// <param name="lotteMovieSession">Lotte movie seesion model</param>
        /// <param name="moveId">Lotte movie id</param>
        /// <param name="cinemaAddressDict">Lotte cinema address dictionary (key: CinemaID)</param>
        /// <returns>App Movie model</returns>
        public static MovieSession MapToMovieSession(
            LotteMovieSession lotteMovieSession,
            string movieId,
            Dictionary<string, string> cinemaAddressDict)
        {
            return new MovieSession
            {
                MovieId = movieId,
                Location = cinemaAddressDict.ContainsKey(lotteMovieSession.CinemaID) ?
                    cinemaAddressDict[lotteMovieSession.CinemaID] : string.Empty,
                ShowDate = lotteMovieSession.PlayDt,
                ShowTime = lotteMovieSession.StartTime
            };
        }
        #endregion
    }
}
