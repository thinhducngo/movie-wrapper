using MovieWrapper.Models;
using MovieWrapper.Vendors.GalaxyCinema.Models;
using MovieWrapper.Vendors.LotteCinema.Models;

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
        /// <returns>App Movie session model</returns>
        public static MovieSession MapToMovieSession(GalaxyMovieSessionItem galaxyMovieSession)
        {
            return new MovieSession
            {
                Location = galaxyMovieSession.Address,
                ShowDate = galaxyMovieSession.ShowDate,
                ShowTime = galaxyMovieSession.ShowTime
                /* Dates = galaxyMovieSession.Dates
                    .SelectMany(date => date.Bundles.SelectMany(bundle => bundle.Sessions.Select(x => new MovieSessionDate
                    {
                        ShowDate = x.ShowDate,
                        ShowTime = x.ShowTime
                    })))
                    .ToList()    */
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
        /// <returns>App Movie model</returns>
        public static MovieSession MapToMovieSession(LotteMovieSession lotteMovieSession)
        {
            return new MovieSession
            {
                ShowDate = lotteMovieSession.PlayDt,
                ShowTime = lotteMovieSession.StartTime
                /* Dates = lotteMovieSession
                    .SelectMany(date => date.Bundles.SelectMany(bundle => bundle.Sessions.Select(x => new MovieSessionDate
                    {
                        ShowDate = x.ShowDate,
                        ShowTime = x.ShowTime
                    })))
                    .ToList() */
            };
        }
        #endregion
    }
}
