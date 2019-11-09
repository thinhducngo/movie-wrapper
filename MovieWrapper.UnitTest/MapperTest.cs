using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieWrapper.Utils;
using MovieWrapper.Vendors.GalaxyCinema.Models;
using MovieWrapper.Vendors.LotteCinema.Models;
using System;
using System.Collections.Generic;

namespace MovieWrapper.UnitTest
{
    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void Test_MapToMovieFromGalaxyMovie()
        {
            // arrange
            var galaxyMovie = new GalaxyMovie
            {
                Id = new Guid("fad62b29-a4ae-4e37-bb3e-b43e1236d488"),
                Name = "Galaxy movie name",
                Startdate = new DateTime(2019, 11, 29),
                Point = (decimal)7.82,
                Description = "Description Galaxy movie"
            };

            // action
            var movie = Mapper.MapToMovie(galaxyMovie);

            // assert
            Assert.AreEqual("fad62b29-a4ae-4e37-bb3e-b43e1236d488", movie.Id);
            Assert.AreEqual("Galaxy movie name", movie.Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), movie.ReleaseDate);
            Assert.AreEqual((decimal)7.82, movie.Rating);
            Assert.AreEqual("Description Galaxy movie", movie.Description);
        }

        [TestMethod]
        public void Test_MapToMovieFromLotteMovieShortView()
        {
            // arrange
            var lotteMovieShortView = new LotteMovieShortView
            {
                RepresentationMovieCode = "32562",
                MovieName = "Galaxy movie name",
                ReleaseDate = "20191129",
                ViewEvaluation = (decimal)8.37
            };

            // action
            var movie = Mapper.MapToMovie(lotteMovieShortView);

            // assert
            Assert.AreEqual("32562", movie.Id);
            Assert.AreEqual("Galaxy movie name", movie.Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), movie.ReleaseDate);
            Assert.AreEqual((decimal)8.37, movie.Rating);
            Assert.IsNull(movie.Description);
        }

        [TestMethod]
        public void Test_MapToMovieFromLotteMovie()
        {
            // arrange
            var lotteMovieShortView = new LotteMovie
            {
                RepresentationMovieCode = "32562",
                MovieName = "Galaxy movie name",
                ReleaseDate = "20191129",
                ViewEvaluation = (decimal)8.37,
                Synopsis = "Description Lotte movie"
            };

            // action
            var movie = Mapper.MapToMovie(lotteMovieShortView);

            // assert
            Assert.AreEqual("32562", movie.Id);
            Assert.AreEqual("Galaxy movie name", movie.Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), movie.ReleaseDate);
            Assert.AreEqual((decimal)8.37, movie.Rating);
            Assert.AreEqual("Description Lotte movie", movie.Description);
        }

        [TestMethod]
        public void Test_MapToMovieSessionFromGalaxyMovieSession()
        {
            // arrange
            var movieId = "fad62b29-a4ae-4e37-bb3e-b43e1236d488";
            var galaxyMovieSession = new GalaxyMovieSessionItem
            {
                Address = "Galaxy cinema address",
                ShowDate = "20191129",
                ShowTime = "2015"
            };

            // action
            var movieSession = Mapper.MapToMovieSession(galaxyMovieSession, movieId);

            // assert
            Assert.AreEqual("fad62b29-a4ae-4e37-bb3e-b43e1236d488", movieSession.MovieId);
            Assert.AreEqual("Galaxy cinema address", movieSession.Location);
            Assert.AreEqual("20191129", movieSession.ShowDate);
            Assert.AreEqual("2015", movieSession.ShowTime);
        }

        [TestMethod]
        public void Test_MapToMovieSessionFromLotteMovieSession_CanMapAddress()
        {
            // arrange
            var movieId = "34554";
            var addressDict = new Dictionary<string, string>
            {
                { "1007", "Cinema 1" },
                { "1008", "Cinema 2" },
                { "1009", "Cinema 3" },
            };
            var lotteMovieSession = new LotteMovieSession
            {
                Address = "Galaxy cinema address",
                CinemaID = "1008",
                PlayDt = "20191129",
                StartTime = "20:15"
            };

            // action
            var movieSession = Mapper.MapToMovieSession(lotteMovieSession, movieId, addressDict);

            // assert
            Assert.AreEqual("34554", movieSession.MovieId);
            Assert.AreEqual("Cinema 2", movieSession.Location);
            Assert.AreEqual("20191129", movieSession.ShowDate);
            Assert.AreEqual("20:15", movieSession.ShowTime);
        }

        [TestMethod]
        public void Test_MapToMovieSessionFromLotteMovieSession_CanNotMapAddress()
        {
            // arrange
            var movieId = "34554";
            var addressDict = new Dictionary<string, string>
            {
                { "1007", "Cinema 1" },
                { "1008", "Cinema 2" },
                { "1009", "Cinema 3" },
            };
            var lotteMovieSession = new LotteMovieSession
            {
                Address = "Galaxy cinema address",
                CinemaID = "1006",
                PlayDt = "20191129",
                StartTime = "20:15"
            };

            // action
            var movieSession = Mapper.MapToMovieSession(lotteMovieSession, movieId, addressDict);

            // assert
            Assert.AreEqual("34554", movieSession.MovieId);
            Assert.AreEqual("", movieSession.Location);
            Assert.AreEqual("20191129", movieSession.ShowDate);
            Assert.AreEqual("20:15", movieSession.ShowTime);
        }
    }
}
