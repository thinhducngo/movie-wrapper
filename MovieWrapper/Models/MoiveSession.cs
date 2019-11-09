using System.Collections.Generic;

namespace MovieWrapper.Models
{
    public class MovieSession
    {
        public string MovieId { get; set; }
        public string Location { get; set; }
        public string ShowDate { get; set; }
        public string ShowTime { get; set; }
    }

    public class MovieSessionListResult : ServiceResult<IList<MovieSession>> { }
}
