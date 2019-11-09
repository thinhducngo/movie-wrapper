using System.Collections.Generic;

namespace MovieWrapper.Vendors.GalaxyCinema.Models
{
    public class GalaxySessionDate
    {
        public string ShowDate { get; set; }
        public string DayOfWeekLabel { get; set; }
        public List<GalaxySessionDateBundle> Bundles { get; set; }
    }

    public class GalaxySessionDateBundle
    {
        public string Caption { get; set; }
        public string Code { get; set; }
        public List<BundleSession> Sessions { get; set; }
        public string Version { get; set; }
    }

    public class BundleSession
    {
        public string Id { get; set; }
        public string ShowDate { get; set; }
        public string ShowTime { get; set; }
    }
}
