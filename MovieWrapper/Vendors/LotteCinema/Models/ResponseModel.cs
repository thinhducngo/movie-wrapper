using System.Collections.Generic;

namespace MovieWrapper.Vendors.LotteCinema.Models
{
    public abstract class BaseResponse
    {
        public string IsOK { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }

    public class DataItem<T>
    {
        public int? ItemCount { get; set; }
        public List<T> Items { get; set; }
    }

    public class GetMoviesResponse : BaseResponse
    {
        public DataItem<LotteMovieShortView> Movies { get; set; }
    }

    public class GetMovieDetailResponse : BaseResponse
    {
        public LotteMovie Movie { get; set; }
    }

    public class GetTicketingPageResponse : BaseResponse
    {
        public GetTicketingPageCinema Cinemas { get; set; }
        public GetTicketingPageDate MoviePlayDates { get; set; }
    }

    public class GetTicketingPageCinema : BaseResponse
    {
        public DataItem<LotteCinemaItem> Cinemas { get; set; }
    }

    public class LotteCinemaItem
    {
        public string CinemaID { get; set; }
        public string CinemaName { get; set; }
        public string DivisionCode { get; set; }
        public string DetailDivisionCode { get; set; }
        public string Address { get; set; }
    }

    public class GetTicketingPageDate : BaseResponse
    {
        public DataItem<LottePlayDate> Items { get; set; }
    }

    public class LottePlayDate
    {
        public string PlayDate { get; set; }
        public string IsPlayDate { get; set; }
    }

    public class GetPlaySequenceResponse : BaseResponse
    {
        public DataItem<LotteMovieSession> PlaySeqs { get; set; }
    }

    public class GetCinemaDetailItem : BaseResponse
    {
        public LotteCinemaItem CinemaDetail { get; set; }
    }
}
