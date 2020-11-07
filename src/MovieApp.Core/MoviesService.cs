using System.Collections.Generic;

namespace MovieApp.Core
{
    public class MoviesService : IMoviesService
    {
        public IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie
                {
                    Name = "THOR",
                    Rating = 4.3,
                    NumberOfRatings = 1896,
                    Description = "2011 – Marvel Studios",
                    BackgroundImage = "thorBack.jpg",
                    HeroImage = "thor.png",
                    FirstDetailImage = "thorLightning2.png",
                    SecondDetailImage = "thorLightning1.png"
                },
                new Movie
                {
                    Name = "DR.STRANGE",
                    Rating = 4.5,
                    NumberOfRatings = 2310,
                    Description = "2016 – Marvel Studios",
                    BackgroundImage = "strangeBack.jpg",
                    HeroImage = "strange.png",
                    FirstDetailImage = "strangeLightning2.png",
                    SecondDetailImage = "strangeLightning1.png"
                },
                new Movie
                {
                    Name = "IRON MAN 2",
                    Rating = 4.8,
                    NumberOfRatings = 1670,
                    Description = "2010 – Marvel Studios",
                    BackgroundImage = "ironmanBack.jpg",
                    HeroImage = "ironman.png",
                    FirstDetailImage = "",
                    SecondDetailImage = "",
                    ThirdDetailImage = "light.png"
                }
            };
        }
    }
}
