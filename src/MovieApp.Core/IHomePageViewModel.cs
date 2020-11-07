using System.Collections.Generic;
using System.ComponentModel;

namespace MovieApp.Core
{
    public interface IHomePageViewModel
    {
        IEnumerable<Movie> Movies { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}