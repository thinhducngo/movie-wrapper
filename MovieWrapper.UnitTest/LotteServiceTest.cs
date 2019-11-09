using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieWrapper.Vendors.LotteCinema.Models;
using MovieWrapper.Vendors.Lotteria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieWrapper.UnitTest
{
    [TestClass]
    public class LotteServiceTest
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
            var responseData = new GetMoviesResponse
            {
                IsOK = "true",
                ResultMessage = "SUCCESS",
                Movies = new DataItem<LotteMovieShortView>
                {
                    ItemCount = 2,
                    Items = new List<LotteMovieShortView>
                    {
                        new LotteMovieShortView
                        {
                            RepresentationMovieCode = "10001",
                            MovieName = "Movie 1",
                            ReleaseDate = "20191129",
                            ViewEvaluation =  (decimal)7.34
                        },
                        new LotteMovieShortView
                        {
                            RepresentationMovieCode = "10002",
                            MovieName = "Movie 2",
                            ReleaseDate = "20191130",
                            ViewEvaluation =  (decimal)8.82
                        }
                    }
                }
            };
            _requesterMock
                .Setup(x => x.Post<GetMoviesResponse>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetShowingMovies();

            // assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Data.Count);

            Assert.AreEqual("10001", response.Data[0].Id);
            Assert.AreEqual("Movie 1", response.Data[0].Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), response.Data[0].ReleaseDate);
            Assert.AreEqual((decimal)7.34, response.Data[0].Rating);
            Assert.IsNull(response.Data[0].Description);

            Assert.AreEqual("10002", response.Data[1].Id);
            Assert.AreEqual("Movie 2", response.Data[1].Name);
            Assert.AreEqual(new DateTime(2019, 11, 30), response.Data[1].ReleaseDate);
            Assert.AreEqual((decimal)8.82, response.Data[1].Rating);
            Assert.IsNull(response.Data[1].Description);
        }

        [TestMethod]
        public async Task Test_GetShowingMovies_Failed()
        {
            // arrange
            _requesterMock
                .Setup(x => x.Post<GetMoviesResponse>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Request failed"));

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetShowingMovies();

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Request failed", response.Message);
        }

        [TestMethod]
        public async Task Test_GetShowingMovies_NoSuccess()
        {
            // arrange
            var responseData = new GetMoviesResponse
            {
                IsOK = "false",
                ResultMessage = "ERROR"
            };
            _requesterMock
                .Setup(x => x.Post<GetMoviesResponse>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetShowingMovies();

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("ERROR", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieDetails_Success()
        {
            // arrange
            var responseData = new GetMovieDetailResponse
            {
                IsOK = "true",
                ResultMessage = "SUCCESS",
                Movie = new LotteMovie
                {
                    RepresentationMovieCode = "10001",
                    MovieName = "Movie 1",
                    ReleaseDate = "20191129",
                    ViewEvaluation = (decimal)7.34,
                    Synopsis = "Description 1"
                }
            };
            _requesterMock
               .Setup(x => x.Post<GetMovieDetailResponse>(
                   It.IsAny<string>(), 
                   It.Is<string>(y => y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieDetails("10001");

            // assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual("10001", response.Data.Id);
            Assert.AreEqual("Movie 1", response.Data.Name);
            Assert.AreEqual(new DateTime(2019, 11, 29), response.Data.ReleaseDate);
            Assert.AreEqual((decimal)7.34, response.Data.Rating);
            Assert.AreEqual("Description 1", response.Data.Description);
        }

        [TestMethod]
        public async Task Test_GetMovieDetails_Failed()
        {
            // arrange
            _requesterMock
               .Setup(x => x.Post<GetMovieDetailResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y => y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ThrowsAsync(new Exception("Request failed"));

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieDetails("10001");

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Request failed", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieDetails_NoSuccess()
        {
            // arrange
            var responseData = new GetMovieDetailResponse
            {
                IsOK = "false",
                ResultMessage = "NOT FOUND"
            };
            _requesterMock
                .Setup(x => x.Post<GetMovieDetailResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y => y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieDetails("10001");

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("NOT FOUND", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieSessions_Success()
        {
            // arrange
            var responseData = new GetTicketingPageResponse
            {
                IsOK = "true",
                ResultMessage = "SUCCESS",
                MoviePlayDates = new GetTicketingPageDate
                {
                    IsOK = "true",
                    ResultMessage = "SUCCESS",
                    Items = new DataItem<LottePlayDate>
                    {
                        ItemCount = 2,
                        Items = new List<LottePlayDate>
                        {
                            new LottePlayDate
                            {
                                IsPlayDate = "Y",
                                PlayDate = "20191129"
                            },
                            new LottePlayDate
                            {
                                IsPlayDate = "N",
                                PlayDate = "20191130"
                            }
                        }
                    }
                },
                Cinemas = new GetTicketingPageCinema
                {
                    IsOK = "true",
                    ResultMessage = "SUCCESS",
                    Cinemas = new DataItem<LotteCinemaItem>
                    {
                        ItemCount = 2,
                        Items = new List<LotteCinemaItem>
                        {
                            new LotteCinemaItem
                            {
                                CinemaID = "8000",
                                DivisionCode = "1",
                                DetailDivisionCode = "0001",
                            },
                            new LotteCinemaItem
                            {
                                CinemaID = "8001",
                                DivisionCode = "2",
                                DetailDivisionCode = "0002",
                            }
                        }
                    }
                }
            };
            _requesterMock
               .Setup(x => x.Post<GetTicketingPageResponse>(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.IsAny<string>()))
               .ReturnsAsync(responseData);
            _requesterMock
                .Setup(x => x.Post<GetCinemaDetailItem>(
                    It.IsAny<string>(),
                    It.Is<string>(y =>
                        y.Contains($"divisionCode:'1'") &&
                        y.Contains($"detailDivisionCode:'0001'") &&
                        y.Contains($"cinemaID:'8000'")),
                    It.IsAny<string>()))
                .ReturnsAsync(new GetCinemaDetailItem
                {
                    IsOK = "true",
                    ResultMessage = "SUCCESS",
                    CinemaDetail = new LotteCinemaItem
                    {
                        CinemaID = "8000",
                        DivisionCode = "1",
                        DetailDivisionCode = "0001",
                        Address = "Cinema 8000"
                    }
                });
            _requesterMock
                .Setup(x => x.Post<GetCinemaDetailItem>(
                    It.IsAny<string>(),
                    It.Is<string>(y =>
                        y.Contains($"divisionCode:'2'") &&
                        y.Contains($"detailDivisionCode:'0002'") &&
                        y.Contains($"cinemaID:'8001'")),
                    It.IsAny<string>()))
                .ReturnsAsync(new GetCinemaDetailItem
                {
                    IsOK = "true",
                    ResultMessage = "SUCCESS",
                    CinemaDetail = new LotteCinemaItem
                    {
                        CinemaID = "8001",
                        DivisionCode = "2",
                        DetailDivisionCode = "0002",
                        Address = "Cinema 8001"
                    }
                });
            _requesterMock
               .Setup(x => x.Post<GetPlaySequenceResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y =>
                        y.Contains("MethodName:'GetPlaySequence'") &&
                        y.Contains("playDate:'20191129'") &&
                        y.Contains("cinemaID:'1|0001|8000'") &&
                        y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ReturnsAsync(new GetPlaySequenceResponse
               {
                   IsOK = "true",
                   ResultMessage = "SUCCESS",
                   PlaySeqs = new DataItem<LotteMovieSession>
                   {
                       ItemCount = 2,
                       Items = new List<LotteMovieSession>
                       {
                           new LotteMovieSession
                           {
                               CinemaID = "8000",
                               PlayDt = "20191129",
                               StartTime = "1945"
                           },
                           new LotteMovieSession
                           {
                               CinemaID = "8000",
                               PlayDt = "20191129",
                               StartTime = "2030"
                           }
                       }
                   }
               });
            _requesterMock
               .Setup(x => x.Post<GetPlaySequenceResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y =>
                        y.Contains("MethodName:'GetPlaySequence'") &&
                        y.Contains("playDate:'20191129'") &&
                        y.Contains("cinemaID:'2|0002|8001'") &&
                        y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ReturnsAsync(new GetPlaySequenceResponse
               {
                   IsOK = "true",
                   ResultMessage = "SUCCESS",
                   PlaySeqs = new DataItem<LotteMovieSession>
                   {
                       ItemCount = 2,
                       Items = new List<LotteMovieSession>
                       {
                           new LotteMovieSession
                           {
                               CinemaID = "8001",
                               PlayDt = "20191129",
                               StartTime = "1700"
                           }
                       }
                   }
               });

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieSessions("10001");

            // assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(3, response.Data.Count);

            Assert.AreEqual("10001", response.Data[0].MovieId);
            Assert.AreEqual("Cinema 8000", response.Data[0].Location);
            Assert.AreEqual("20191129", response.Data[0].ShowDate);
            Assert.AreEqual("1945", response.Data[0].ShowTime);

            Assert.AreEqual("10001", response.Data[1].MovieId);
            Assert.AreEqual("Cinema 8000", response.Data[1].Location);
            Assert.AreEqual("20191129", response.Data[1].ShowDate);
            Assert.AreEqual("2030", response.Data[1].ShowTime);

            Assert.AreEqual("10001", response.Data[2].MovieId);
            Assert.AreEqual("Cinema 8001", response.Data[2].Location);
            Assert.AreEqual("20191129", response.Data[2].ShowDate);
            Assert.AreEqual("1700", response.Data[2].ShowTime);
        }

        [TestMethod]
        public async Task Test_GetMovieSessions_Failed()
        {
            // arrange
            _requesterMock
               .Setup(x => x.Post<GetTicketingPageResponse>(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.IsAny<string>()))
               .ThrowsAsync(new Exception("Request failed"));

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieSessions("10001");

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Request failed", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieSessions_NoSuccess()
        {
            // arrange
            var responseData = new GetTicketingPageResponse
            {
                IsOK = "false",
                ResultMessage = "ERROR"
            };
            _requesterMock
               .Setup(x => x.Post<GetTicketingPageResponse>(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.IsAny<string>()))
               .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieSessions("10001");

            // assert
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual("ERROR", response.Message);
        }

        [TestMethod]
        public async Task Test_GetMovieSession_Success()
        {
            // arrange
            var responseData = new GetPlaySequenceResponse
            {
                IsOK = "true",
                ResultMessage = "SUCCESS",
                PlaySeqs = new DataItem<LotteMovieSession>
                {
                    ItemCount = 2,
                    Items = new List<LotteMovieSession>
                    {
                        new LotteMovieSession
                        {
                            CinemaID = "8008",
                            PlayDt = "20191129",
                            StartTime = "1945"
                        },
                        new LotteMovieSession
                        {
                            CinemaID = "8008",
                            PlayDt = "20191129",
                            StartTime = "2015"
                        }
                    }
                }
            };
            _requesterMock
               .Setup(x => x.Post<GetPlaySequenceResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y =>
                        y.Contains("MethodName:'GetPlaySequence'") &&
                        y.Contains("playDate:'20191129'") &&
                        y.Contains("cinemaID:'1|0001|8008'") &&
                        y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieSession("10001", "20191129", "8008", "1", "0001");

            // assert
            Assert.AreEqual(2, response.Count);

            Assert.AreEqual("8008", response[0].CinemaID);
            Assert.AreEqual("20191129", response[0].PlayDt);
            Assert.AreEqual("1945", response[0].StartTime);

            Assert.AreEqual("8008", response[1].CinemaID);
            Assert.AreEqual("20191129", response[1].PlayDt);
            Assert.AreEqual("2015", response[1].StartTime);
        }

        [TestMethod]
        public async Task Test_GetMovieSession_Failed()
        {
            // arrange
            _requesterMock
               .Setup(x => x.Post<GetPlaySequenceResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y =>
                        y.Contains("MethodName:'GetPlaySequence'") &&
                        y.Contains("playDate:'20191129'") &&
                        y.Contains("cinemaID:'1|0001|8008'") &&
                        y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ThrowsAsync(new Exception("Request failed"));

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieSession("10001", "20191129", "8008", "1", "0001");

            // assert
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public async Task Test_GetMovieSession_NoSuccess()
        {
            // arrange
            var responseData = new GetPlaySequenceResponse
            {
                IsOK = "false",
                ResultMessage = "ERROR"
            };
            _requesterMock
               .Setup(x => x.Post<GetPlaySequenceResponse>(
                   It.IsAny<string>(),
                   It.Is<string>(y =>
                        y.Contains("MethodName:'GetPlaySequence'") &&
                        y.Contains("playDate:'20191129'") &&
                        y.Contains("cinemaID:'1|0001|8008'") &&
                        y.Contains("representationMovieCode:'10001'")),
                   It.IsAny<string>()))
               .ReturnsAsync(responseData);

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetMovieSession("10001", "20191129", "8008", "1", "0001");

            // assert
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public async Task Test_GetCinemaAddressDict()
        {
            // arrange
            var data = new List<LotteCinemaItem>
            {
                new LotteCinemaItem
                {
                    CinemaID = "8000",
                    DivisionCode = "1",
                    DetailDivisionCode = "0001",
                },
                new LotteCinemaItem
                {
                    CinemaID = "8001",
                    DivisionCode = "1",
                    DetailDivisionCode = "0001",
                },
                new LotteCinemaItem
                {
                    CinemaID = "8000",
                    DivisionCode = "2",
                    DetailDivisionCode = "0002",
                }
            };
            foreach (var cinema in data)
            {
                _requesterMock
                    .Setup(x => x.Post<GetCinemaDetailItem>(
                       It.IsAny<string>(),
                       It.Is<string>(y =>
                            y.Contains($"divisionCode:'{cinema.DivisionCode}'") &&
                            y.Contains($"detailDivisionCode:'{cinema.DetailDivisionCode}'") &&
                            y.Contains($"cinemaID:'{cinema.CinemaID}'")),
                       It.IsAny<string>()))
                   .ReturnsAsync(new GetCinemaDetailItem
                   {
                       IsOK = "true",
                       ResultMessage = "SUCCESS",
                       CinemaDetail = new LotteCinemaItem
                       {
                           CinemaID = cinema.CinemaID,
                           DivisionCode = cinema.DivisionCode,
                           DetailDivisionCode = cinema.DetailDivisionCode,
                           Address = $"Cinema {cinema.CinemaID}"
                       }
                   });
            }

            // action
            var service = new LotteService(_requesterMock.Object);
            var response = await service.GetCinemaAddressDict(data);

            // assert
            _requesterMock.Verify(m => m.Post<GetCinemaDetailItem>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(2, response.Count);

            Assert.IsTrue(response.ContainsKey("8000"));
            Assert.AreEqual("Cinema 8000", response["8000"]);

            Assert.IsTrue(response.ContainsKey("8001"));
            Assert.AreEqual("Cinema 8001", response["8001"]);
        }

        //[TestMethod]
        /* public async Task Test_GetMovieSessions_Success()
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

        // [TestMethod]
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

        //[TestMethod]
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
        } */
    }
}
