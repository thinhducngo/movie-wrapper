using System;

namespace MovieWrapper.Vendors.GalaxyCinema.Models
{
    /// <summary>
    /// Always consider variable value is null.
    /// Another related id (ex: PreviewId, ReviewId, ...): assume they are string type
    /// </summary>
    public class GalaxyMovie : GalaxyBaseModel
    {
        public string Age { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public string ImageLandscape { get; set; }
        public string ImageLandscapeMobile { get; set; }
        public string ImagePortrait { get; set; }
        public string Name { get; set; }
        public decimal? Point { get; set; }
        public string PreviewId { get; set; }
        public string ReviewId { get; set; }
        public string SEOId { get; set; }
        public string Slug { get; set; }
        public DateTime? Startdate { get; set; }
        public int? Status { get; set; }
        public string SubName { get; set; }
        public int? TotalVotes { get; set; }
        public string Trailer { get; set; }
        public int? Views { get; set; }
    }
}
