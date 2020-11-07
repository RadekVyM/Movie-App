using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieView : ContentView, IDisposable
    {
        bool animationIsActive = false;
        Random random;
        IAccelerometerService accelerometerService;

        public bool IsExpanded { get; set; } = false;

        public MovieView() : this(false)
        {
        }

        public MovieView(bool isFake)
        {
            if(!isFake)
                random = new Random();

            InitializeComponent();

            accelerometerService = ((App)App.Current).ServiceProvider.GetRequiredService<IAccelerometerService>();
            accelerometerService.TranslationChanged += TranslationChanged;

            if (!isFake)
                _ = StartAnimationsAsync();

            if(isFake)
            {
                firstDetailImage.IsVisible = false;
                secondDetailImage.IsVisible = false;
                thirdDetailImage.IsVisible = false;
                //infoStack.IsVisible = false;

                firstDetailImage.Source = null;
                secondDetailImage.Source = null;
                thirdDetailImage.Source = null;
            }
        }

        private void TranslationChanged(double x, double y)
        {
            if (x == 0)
                return;

            double heroTranslationX = x * 1.5d;

            var heroOffset = ((heroImage.Width * heroImage.Scale) - Width) / 2d;

            if (heroTranslationX < 0 && heroTranslationX < -heroOffset)
                heroTranslationX = -heroOffset + 4;

            if (heroTranslationX > 0 && heroTranslationX > heroOffset)
                heroTranslationX = heroOffset - 4;

            double backTranslationX = x * 0.5d;

            var backOffset = ((backImage.Width * backImage.Scale) - Width) / 2d;

            if (backTranslationX < 0 && backTranslationX < -backOffset)
                backTranslationX = -backOffset + 4;

            if (backTranslationX > 0 && backTranslationX > backOffset)
                backTranslationX = backOffset - 4;

            Task.WhenAll(
                backImage.TranslateTo(backTranslationX, 0, 150),
                heroImage.TranslateTo(heroTranslationX, 0, 150),
                thirdDetailImage.TranslateTo(heroTranslationX, 0, 150)
                );
        }

        private async Task StartAnimationsAsync()
        {
            animationIsActive = true;

            await Task.Delay(random.Next(0, 3) * 1000);

            Device.StartTimer(TimeSpan.FromSeconds(random.Next(6, 8)), () =>
            {
                firstDetailImage.FadeTo(1).ContinueWith( async _ => 
                {
                    await Task.Delay(500);
                    await firstDetailImage.FadeTo(0);

                    await Task.Delay(random.Next(2, 5) * 1000);

                    await secondDetailImage.FadeTo(1);
                    await Task.Delay(500);
                    _ = secondDetailImage.FadeTo(0);
                });

                return animationIsActive;
            });

            Device.StartTimer(TimeSpan.FromMilliseconds(600), () =>
            {
                thirdDetailImage.FadeTo(1, 150).ContinueWith(async _ =>
                {
                    await Task.Delay(random.Next(1, 4) * 100);
                    _ = thirdDetailImage.FadeTo(0.3, 150);
                });

                return animationIsActive;
            });
        }

        private void StopAnimations()
        {
            animationIsActive = false;
        }

        public async Task Expand()
        {
            await Task.WhenAll(
                heroImage.ScaleTo(1.5, easing: Easing.SpringOut),
                infoStack.FadeTo(0)
                );
        }

        public async Task Shrink()
        {
            await Task.WhenAll(
                heroImage.ScaleTo(1.4, easing: Easing.SpringOut),
                infoStack.FadeTo(1)
                );
        }

        public void Dispose()
        {
            accelerometerService.TranslationChanged -= TranslationChanged;
            accelerometerService = null;
        }
    }
}