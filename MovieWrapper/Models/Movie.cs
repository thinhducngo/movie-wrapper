using System;
using System.Collections.Generic;

namespace MovieWrapper.Models
{
    public class Movie : BaseModel
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }
    }

    public class MovieResult : ServiceResult<Movie> { }
    public class MovieListResult : ServiceResult<IList<Movie>> { }
}
