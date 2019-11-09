using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieWrapper.Vendors.GalaxyCinema;
using MovieWrapper.Vendors.GalaxyCinema.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieWrapper.UnitTest
{
    [TestClass]
    public class GalaxyServiceTest
    {
        private Mock<IRequester> _requesterMock;

        [TestInitialize]
        public void Initialize()
        {
            _requesterMock = new Mock<IRequester>();
        }

        [TestMethod]
        public async Task Test_GetShowingMovies_Success()
        {
            // arrange
            var responseData = new ShowAndComingModel
            {
                MovieCommingSoon = new List<GalaxyMovie>
                    {
                        new GalaxyMovie
                        {
                            Id = new Guid("555f43a1-6b61-4dcd-8ef8-43e61e2b9df1"),
                            Name = "Coming 1",
                            Startdate = new DateTime(2019, 11, 29),
                            Point = (decimal)7.82,
                            Description = "Description coming 1"
                        }
                    },
                MovieShowing = new List<GalaxyMovie>
                    {
                        new GalaxyMovie
                        {
                            Id = new Guid("43c5a775-0075-4b2e-87e9-7e792ecfddf9"),
                            Name = "Showing 1",
                            Startdate = new DateTime(2019, 11, 30),
                            Point = (decimal)8.42,
                            Description = "Description showing 1"
                        },
                        new GalaxyMovie
                        {
                            Id = new Guid("89dcbad0-a998-48b2-ab59-ec25adc95efd"),
                            Name = "Showing 2",
                            Startdate = new DateTime(2019, 11, 29),
                            Point = (decimal)7.82,
                            Description = "Description showing 2"
                        }
                    }
            };
            _requesterMock
                .Setup(x => x.Get<ShowAndComingModel>(It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetShowingMovies();

            // assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Data.Count);
            Assert.AreEqual("43c5a775-0075-4b2e-87e9-7e792ecfddf9", response.Data[0].Id);

            Assert.AreEqual("43c5a775-0075-4b2e-87e9-7e792ecfddf9", response.Data[0].Id);
            Assert.AreEqual("Showing 1", response.Data[0].Name);
            Assert.AreEqual(new DateTime(2019, 11, 30), response.Data[0].ReleaseDate);
            Assert.AreEqual((decimal)8.42, response.Data[0].Rating);
            Assert.AreEqual("Description showing 1", response.Data[0].Description);

            Assert.AreEqual("89dcbad0-a998-48b2-ab59-ec25adc95efd", response.Data[1].Id);
            Assert.AreEqual("Showing 2", response.Data[1].Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), response.Data[1].ReleaseDate);
            Assert.AreEqual((decimal)7.82, response.Data[1].Rating);
            Assert.AreEqual("Description showing 2", response.Data[1].Description);
        }

        [TestMethod]
        public async Task Test_GetShowingMovies_Failed()
        {
            // arrange
            _requesterMock
                .Setup(x => x.Get<ShowAndComingModel>(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Request failed"));

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetShowingMovies();

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Request failed", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieDetails_Success()
        {
            // arrange
            var responseData = new ShowAndComingModel
            {
                MovieCommingSoon = new List<GalaxyMovie>
                    {
                        new GalaxyMovie
                        {
                            Id = new Guid("555f43a1-6b61-4dcd-8ef8-43e61e2b9df1"),
                            Name = "Coming 1",
                            Startdate = new DateTime(2019, 11, 29),
                            Point = (decimal)7.82,
                            Description = "Description coming 1"
                        }
                    },
                MovieShowing = new List<GalaxyMovie>
                    {
                        new GalaxyMovie
                        {
                            Id = new Guid("43c5a775-0075-4b2e-87e9-7e792ecfddf9"),
                            Name = "Showing 1",
                            Startdate = new DateTime(2019, 11, 30),
                            Point = (decimal)8.42,
                            Description = "Description showing 1"
                        },
                        new GalaxyMovie
                        {
                            Id = new Guid("89dcbad0-a998-48b2-ab59-ec25adc95efd"),
                            Name = "Showing 2",
                            Startdate = new DateTime(2019, 11, 29),
                            Point = (decimal)7.82,
                            Description = "Description showing 2"
                        }
                    }
            };
            _requesterMock
                .Setup(x => x.Get<ShowAndComingModel>(It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetMovieDetails("89dcbad0-a998-48b2-ab59-ec25adc95efd");

            // assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("89dcbad0-a998-48b2-ab59-ec25adc95efd", response.Data.Id);
            Assert.AreEqual("Showing 2", response.Data.Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), response.Data.ReleaseDate);
            Assert.AreEqual((decimal)7.82, response.Data.Rating);
            Assert.AreEqual("Description showing 2", response.Data.Description);
        }

        [TestMethod]
        public async Task Test_GetMovieDetails_Failed()
        {
            // arrange
            _requesterMock
                .Setup(x => x.Get<ShowAndComingModel>(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Request failed"));

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetShowingMovies();

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Request failed", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieDetails_NotFound()
        {
            // arrange
            var responseData = new ShowAndComingModel
            {
                MovieCommingSoon = new List<GalaxyMovie>
                    {
                        new GalaxyMovie
                        {
                            Id = new Guid("555f43a1-6b61-4dcd-8ef8-43e61e2b9df1"),
                            Name = "Coming 1",
                            Startdate = new DateTime(2019, 11, 29),
                            Point = (decimal)7.82,
                            Description = "Description coming 1"
                        }
                    },
                MovieShowing = new List<GalaxyMovie>
                    {
                        new GalaxyMovie
                        {
                            Id = new Guid("43c5a775-0075-4b2e-87e9-7e792ecfddf9"),
                            Name = "Showing 1",
                            Startdate = new DateTime(2019, 11, 30),
                            Point = (decimal)8.42,
                            Description = "Description showing 1"
                        },
                        new GalaxyMovie
                        {
                            Id = new Guid("89dcbad0-a998-48b2-ab59-ec25adc95efd"),
                            Name = "Showing 2",
                            Startdate = new DateTime(2019, 11, 29),
                            Point = (decimal)7.82,
                            Description = "Description showing 2"
                        }
                    }
            };
            _requesterMock
                .Setup(x => x.Get<ShowAndComingModel>(It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetMovieDetails("deba8fa3-1c8e-4351-8185-35819f5759ad");

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Not found", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieSessions_Success()
        {
            // arrange
            var responseData = new List<GalaxyMovieSession>
            {
                new GalaxyMovieSession
                {
                    Address = "Address 1",
                    Dates = new List<GalaxySessionDate>
                    {
                        new GalaxySessionDate
                        {
                            Bundles = new List<GalaxySessionDateBundle>
                            {
                                new GalaxySessionDateBundle
                                {
                                    Sessions = new List<BundleSession>
                                    {
                                        new BundleSession
                                        {
                                            ShowDate = "20191129",
                                            ShowTime = "1215"
                                        },
                                        new BundleSession
                                        {
                                            ShowDate = "20191130",
                                            ShowTime = "2030"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new GalaxyMovieSession
                {
                    Address = "Address 2",
                    Dates = new List<GalaxySessionDate>
                    {
                        new GalaxySessionDate
                        {
                            Bundles = new List<GalaxySessionDateBundle>
                            {
                                new GalaxySessionDateBundle
                                {
                                    Sessions = new List<BundleSession>
                                    {
                                        new BundleSession
                                        {
                                            ShowDate = "20191129",
                                            ShowTime = "1745"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _requesterMock
                .Setup(x => x.Get<List<GalaxyMovieSession>>(It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetMovieSessions("89dcbad0-a998-48b2-ab59-ec25adc95efd");

            // assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(3, response.Data.Count);

            Assert.AreEqual("89dcbad0-a998-48b2-ab59-ec25adc95efd", response.Data[0].MovieId);
            Assert.AreEqual("Address 1", response.Data[0].Location);
            Assert.AreEqual("20191129", response.Data[0].ShowDate);
            Assert.AreEqual("1215", response.Data[0].ShowTime);

            Assert.AreEqual("89dcbad0-a998-48b2-ab59-ec25adc95efd", response.Data[1].MovieId);
            Assert.AreEqual("Address 1", response.Data[1].Location);
            Assert.AreEqual("20191130", response.Data[1].ShowDate);
            Assert.AreEqual("2030", response.Data[1].ShowTime);

            Assert.AreEqual("89dcbad0-a998-48b2-ab59-ec25adc95efd", response.Data[2].MovieId);
            Assert.AreEqual("Address 2", response.Data[2].Location);
            Assert.AreEqual("20191129", response.Data[2].ShowDate);
            Assert.AreEqual("1745", response.Data[2].ShowTime);
        }

        [TestMethod]
        public async Task Test_GetMovieSessions_Failed()
        {
            // arrange
            _requesterMock
                .Setup(x => x.Get<List<GalaxyMovieSession>>(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Request failed"));

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var response = await galaxyService.GetMovieSessions("89dcbad0-a998-48b2-ab59-ec25adc95efd");

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Request failed", response.Message);
        }

        [TestMethod]
        public void Test_SplitSessions()
        {
            // arrange
            var data = new List<GalaxyMovieSession>
            {
                new GalaxyMovieSession
                {
                    Address = "Address 1",
                    Dates = new List<GalaxySessionDate>
                    {
                        new GalaxySessionDate
                        {
                            Bundles = new List<GalaxySessionDateBundle>
                            {
                                new GalaxySessionDateBundle
                                {
                                    Sessions = new List<BundleSession>
                                    {
                                        new BundleSession
                                        {
                                            ShowDate = "20191129",
                                            ShowTime = "1215"
                                        },
                                        new BundleSession
                                        {
                                            ShowDate = "20191130",
                                            ShowTime = "2030"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new GalaxyMovieSession
                {
                    Address = "Address 2",
                    Dates = new List<GalaxySessionDate>
                    {
                        new GalaxySessionDate
                        {
                            Bundles = new List<GalaxySessionDateBundle>
                            {
                                new GalaxySessionDateBundle
                                {
                                    Sessions = new List<BundleSession>
                                    {
                                        new BundleSession
                                        {
                                            ShowDate = "20191129",
                                            ShowTime = "1745"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // action
            var galaxyService = new GalaxyService(_requesterMock.Object);
            var sessions = galaxyService.SplitSessions(data);

            // assert
            Assert.AreEqual(3, sessions.Count);

            Assert.AreEqual("Address 1", sessions[0].Address);
            Assert.AreEqual("20191129", sessions[0].ShowDate);
            Assert.AreEqual("1215", sessions[0].ShowTime);

            Assert.AreEqual("Address 1", sessions[1].Address);
            Assert.AreEqual("20191130", sessions[1].ShowDate);
            Assert.AreEqual("2030", sessions[1].ShowTime);

            Assert.AreEqual("Address 2", sessions[2].Address);
            Assert.AreEqual("20191129", sessions[2].ShowDate);
            Assert.AreEqual("1745", sessions[2].ShowTime);
        }
    }
}
