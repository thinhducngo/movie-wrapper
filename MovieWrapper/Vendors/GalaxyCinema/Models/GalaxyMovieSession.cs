using System;
using System.Collections.Generic;

namespace MovieWrapper.Vendors.GalaxyCinema.Models
{
    /// <summary>
    /// Always consider variable value is null.
    /// Another related id (ex: PreviewId, ReviewId, ...): assume they are string type
    /// </summary>
    public class GalaxyMovieSession : GalaxyBaseModel
    {
        public Guid MovieId { get; set; }

        public string Address { get; set; }
        public string CityId { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<GalaxySessionDate> Dates { get; set; }
        public string Description { get; set; }
        public string ImageLandscape { get; set; }
        public string ImagePortrait { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapEmbeb { get; set; }
        public string Name { get; set; }
        public int? OldId { get; set; }
        public int? Order { get; set; }
        public string Phone { get; set; }
        public string Slug { get; set; }
        public int? Status { get; set; }
        // public List<> Ticket { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class GalaxyMovieSessionItem
    {
        public Guid MovieId { get; set; }
        public string Address { get; set; }
        public string ShowDate { get; set; }
        public string ShowTime { get; set; }
    }
}
