using System.Collections.Generic;

namespace MovieApp.Core
{
    public interface IMoviesService
    {
        IEnumerable<Movie> GetMovies();
    }
}