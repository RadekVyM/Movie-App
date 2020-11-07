using MovieApp.Core;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabBarView : ContentView
    {
        PageEnum selectedPage;

        public TabBarView()
        {
            InitializeComponent();

            _ = UpdateSelection();
        }

        private async void HomeTapped(object sender, EventArgs e)
        {
            selectedPage = PageEnum.HomePage;

            await UpdateSelection();
        }

        private async void SearchTapped(object sender, EventArgs e)
        {
            selectedPage = PageEnum.SearchPage;

            await UpdateSelection();
        }

        private async void ProfileTapped(object sender, EventArgs e)
        {
            selectedPage = PageEnum.ProfilePage;

            await UpdateSelection();
        }

        private async Task UpdateSelection()
        {
            await Task.WhenAll(
                homeGrid.Children[0].FadeTo(selectedPage == PageEnum.HomePage ? 0.5d : 0),
                searchGrid.Children[0].FadeTo(selectedPage == PageEnum.SearchPage ? 0.5d : 0),
                profileGrid.Children[0].FadeTo(selectedPage == PageEnum.ProfilePage ? 0.5d : 0)
                );
        }
    }
}