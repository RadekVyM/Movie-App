using Microsoft.Extensions.DependencyInjection;
using MovieApp.Core;
using System;
using Xamarin.Forms;

namespace MovieApp
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();

            Device.SetFlags(new [] { "Shapes_Experimental", "Brush_Experimental" });

            var services = new ServiceCollection();

            services.AddSingleton<IMoviesService, MoviesService>();
            services.AddSingleton<IAccelerometerService, AccelerometerService>();
            services.AddTransient<IHomePageViewModel, HomePageViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
