using MovieWrapper.Models;
using MovieWrapper.Utils;
using MovieWrapper.Vendors.LotteCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWrapper.Vendors.Lotteria
{
    public class LotteService : IVendorService
    {
        private readonly string _domain;
        private readonly IRequester _requester;

        public LotteService(IRequester requester)
        {
            _domain = "http://www.lottecinemavn.com/LCWS";
            _requester = requester;
        }

        public LotteService(string domain, IRequester requester)
        {
            _domain = domain;
            _requester = requester;
        }

        /// <summary>
        /// Get current showing movies of Lotte cinema
        /// Ref: http://www.lottecinemavn.com/LCHS/Contents/Movie/Movie-List.aspx
        /// </summary>
        /// <returns>List of current showing movie of Lotte cinema with status and message</returns>
        public async Task<MovieListResult> GetShowingMovies()
        {
            // Handle request exception
            try
            {
                var response = await _requester.Post<GetMoviesResponse>(
                    requestUrl: $"{_domain}/Movie/MovieData.aspx",
                    data: "paramList={" + string.Join(",", new[]
                    {
                        "MethodName:'GetMovies'",
                        "channelType:'HO'",
                        "osType:''",
                        "osVersion:''",
                        "multiLanguageID:'LL'",
                        "division:1",
                        "moviePlayYN:'Y'",
                        "orderType:'1'",
                        "blockSize:100",
                        "pageNo:1"
                    }) + "}",
                    contentType: "application/x-www-form-urlencoded");

                if (response.IsOK == "true" && response.ResultMessage == "SUCCESS")
                {
                    return new MovieListResult
                    {
                        Success = true,
                        Data = response.Movies.Items
                            .Select(x => Mapper.MapToMovie(x))
                            .ToList()
                    };
                }
                else
                {
                    return new MovieListResult
                    {
                        Success = false,
                        Message = response.ResultMessage
                    };
                }
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
        /// Get Lotte cinema movie details
        /// Ref: http://www.lottecinemavn.com/LCHS/Contents/Movie/Movie-Detail-View.aspx?movie=10335
        /// </summary>
        /// <param name="movieId">Lotte movie id</param>
        /// <returns>Detail of Lotte cinema movie with status and message</returns>
        public async Task<MovieResult> GetMovieDetails(string movieId)
        {
            // Handle request exception
            try
            {
                var response = await _requester.Post<GetMovieDetailResponse>(
                    requestUrl: $"{_domain}/Movie/MovieData.aspx",
                    data: "paramList={" + string.Join(",", new[]
                    {
                        "MethodName:'GetMovieDetail'",
                        "channelType:'HO'",
                        "osType:''",
                        "osVersion:''",
                        "multiLanguageID:'LL'",
                        $"representationMovieCode:'{movieId}'"
                    }) + "}",
                    contentType: "application/x-www-form-urlencoded");

                if (response.IsOK == "true" && response.ResultMessage == "SUCCESS")
                {
                    return new MovieResult
                    {
                        Success = true,
                        Data = Mapper.MapToMovie(response.Movie)
                    };
                }
                else
                {
                    return new MovieResult
                    {
                        Success = false,
                        Message = response.ResultMessage
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
        /// Get movie sessions of Lotte cinema
        /// Ref: http://www.lottecinemavn.com/LCHS/Contents/ticketing/ticketing.aspx
        /// </summary>
        /// <param name="movieId">Lotte movie id</param>
        /// <returns>List of sessions with status and message with status and message</returns>
        public async Task<MovieSessionListResult> GetMovieSessions(string movieId)
        {
            // Handle request exception
            try
            {
                var response = await _requester.Post<GetTicketingPageResponse>(
                    requestUrl: $"{_domain}/Ticketing/TicketingData.aspx",
                    data: "paramList={" + string.Join(",", new[]
                    {
                        "MethodName:'GetTicketingPage'",
                        "channelType:'HO'",
                        "osType:''",
                        "osVersion:''",
                        "memberOnNo:''"
                    }) + "}",
                    contentType: "application/x-www-form-urlencoded");

                if (response.IsOK == "true" && response.ResultMessage == "SUCCESS")
                {
                    var movieSessions = new List<LotteMovieSession>();
                    var activeDates = response.MoviePlayDates.Items.Items
                        .Where(x => x.IsPlayDate == "Y")
                        .Select(x => x.PlayDate)
                        .ToList();  // Get showing dates only

                    // Cause of no get all sessions api -> loop throught active date and cinema for getting all sessions
                    foreach (var activeDate in activeDates)
                    {
                        foreach (var cinema in response.Cinemas.Cinemas.Items)
                        {
                            movieSessions.AddRange(await GetMovieSession(movieId, activeDate, cinema.CinemaID, cinema.DivisionCode, cinema.DetailDivisionCode));    // Sessions of cinema in active date
                        }
                    }

                    var cinemaAddressDict = await GetCinemaAddressDict(response.Cinemas.Cinemas.Items);

                    return new MovieSessionListResult
                    {
                        Success = true,
                        Data = movieSessions
                            .Select(x => Mapper.MapToMovieSession(x, movieId, cinemaAddressDict))   // Lotte does not return move id
                            .ToList()
                    };
                }
                else
                {
                    return new MovieSessionListResult
                    {
                        Success = false,
                        Message = response.ResultMessage
                    };
                }
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
        /// Get movie session of Lotte cinema
        /// Ref: http://www.lottecinemavn.com/LCHS/Contents/ticketing/ticketing.aspx
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="playDate"></param>
        /// <param name="cinemaID"></param>
        /// <param name="divisionCode"></param>
        /// <param name="detailDivisionCode"></param>
        /// <returns></returns>
        public async Task<IList<LotteMovieSession>> GetMovieSession(
            string movieId, 
            string playDate, 
            string cinemaID,
            string divisionCode,
            string detailDivisionCode)
        {
            try
            {
                var response = await _requester.Post<GetPlaySequenceResponse>(
                    requestUrl: $"{_domain}/Ticketing/TicketingData.aspx",
                    data: "paramList={" + string.Join(",", new[]
                    {
                        "MethodName:'GetPlaySequence'",
                        "channelType:'HO'",
                        "osType:''",
                        "osVersion:''",
                        $"playDate:'{playDate}'",
                        $"cinemaID:'{divisionCode}|{detailDivisionCode}|{cinemaID}'",  // format: {devisionCode}|{detailDivisionCode}|cinemaID (predict)
                        $"representationMovieCode:'{movieId}'"
                    }) + "}",
                    contentType: "application/x-www-form-urlencoded");

                if (response.IsOK == "true" && response.ResultMessage == "SUCCESS") return response.PlaySeqs.Items;
            }
            catch (Exception) { }

            return new List<LotteMovieSession>();
        }

        /// <summary>
        /// Get Lotte cinema address -> create diction for searching
        /// Ref: http://www.lottecinemavn.com/LCHS/Contents/Cinema/Cinema-Detail.aspx
        /// </summary>
        /// <param name="cinemas"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetCinemaAddressDict(List<LotteCinemaItem> cinemas)
        {
            var dict = new Dictionary<string, string>();
            foreach (var cinema in cinemas)
            {
                if (!dict.ContainsKey(cinema.CinemaID)) // advoid duplicate request
                {
                    // Handle exception, get address exception should not affect main action
                    try
                    {
                        var response = await _requester.Post<GetCinemaDetailItem>(
                            requestUrl: $"{_domain}/Cinema/CinemaData.aspx",
                            data: "paramList={" + string.Join(",", new[]
                            {
                                "MethodName:'GetCinemaDetailItem'",
                                "channelType:'HO'",
                                "osType:''",
                                "osVersion:''",
                                $"divisionCode:'{cinema.DivisionCode}'",
                                $"detailDivisionCode:'{cinema.DetailDivisionCode}'",
                                $"cinemaID:'{cinema.CinemaID}'",
                                "memberOnNo:0"
                            }) + "}",
                            contentType: "application/x-www-form-urlencoded");

                        if (response.IsOK == "true" && response.ResultMessage == "SUCCESS")
                        {
                            dict.Add(response.CinemaDetail.CinemaID, response.CinemaDetail.Address);
                        }
                    }
                    catch (Exception) { }
                }
            }
            return dict;
        }
    }
}
