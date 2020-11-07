namespace MovieApp.Core
{
    public class Movie
    {
        public string Name { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public string Description { get; set; }

        public string BackgroundImage { get; set; }
        public string HeroImage { get; set; }
        public string FirstDetailImage { get; set; }
        public string SecondDetailImage { get; set; }
        public string ThirdDetailImage { get; set; }
    }
}
