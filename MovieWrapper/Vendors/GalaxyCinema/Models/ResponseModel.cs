using System.Collections.Generic;

namespace MovieWrapper.Vendors.GalaxyCinema.Models
{
    public class ShowAndComingModel
    {
        public List<GalaxyMovie> MovieCommingSoon { get; set; }
        public List<GalaxyMovie> MovieShowing { get; set; }
    }
}
