using Xamarin.Forms;

namespace MovieApp
{
    public class CustomTabBar : TabBar
    {
        public TabBarView TabBarView { get; private set; }

        public CustomTabBar()
        {
            TabBarView = new TabBarView();

            TabBarView.HeightRequest = 80;
        }
    }
}
