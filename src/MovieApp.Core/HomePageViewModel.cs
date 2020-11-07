using System.Collections.Generic;
using System.ComponentModel;

namespace MovieApp.Core
{
    public class HomePageViewModel : INotifyPropertyChanged, IHomePageViewModel
    {
        private IEnumerable<Movie> movies;

        public IEnumerable<Movie> Movies
        {
            get => movies;
            set
            {
                movies = value;
                OnPropertyChanged(nameof(Movies));
            }
        }

        public HomePageViewModel(IMoviesService moviesService)
        {
            Movies = moviesService.GetMovies();
        }

        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
