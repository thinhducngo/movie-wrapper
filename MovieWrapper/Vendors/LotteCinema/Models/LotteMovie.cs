namespace MovieWrapper.Vendors.LotteCinema.Models
{
    /// <summary>
    /// Always consider variable value is null.
    /// </summary>
    public class LotteMovie
    {
        public string RepresentationMovieCode { get; set; }
        public string MovieName { get; set; }
        public string ReleaseDate { get; set; }
        public decimal? ViewEvaluation { get; set; }
        public string Synopsis { get; set; }
    }


    public class LotteMovieShortView
    {
        public string RepresentationMovieCode { get; set; }
        public string MovieName { get; set; }
        public string ReleaseDate { get; set; }
        public decimal? ViewEvaluation { get; set; }
    }
}
