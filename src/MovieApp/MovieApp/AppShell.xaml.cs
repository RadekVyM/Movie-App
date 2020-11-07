using MovieApp.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            tabBar.Items.Add(new ShellContent { Route = PageEnum.HomePage.ToString(), ContentTemplate = new DataTemplate(typeof(HomePage)) });
        }
    }
}