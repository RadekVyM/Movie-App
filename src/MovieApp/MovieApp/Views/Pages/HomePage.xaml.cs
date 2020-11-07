using Microsoft.Extensions.DependencyInjection;
using MovieApp.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace MovieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        IAccelerometerService accelerometerService;

        double spacing = 36;
        double upperImageDefaultY;
        double bottomImageDefaultY;
        double defaultPositionShiftValue;
        double shrinkedNameLabelY => (Height / 2d) + movieNameLabel.Height;
        double expandedNameLabelY => -(expandedFakeViewHeight / 2d) + movieNameLabel.Height;
        double shrinkedMovieInfoFrameY => (Height / 2d) + movieInfoFrame.Height;
        double expandedMovieInfoFrameY => (expandedFakeViewHeight / 2d) - (movieInfoFrame.Height / 2d) - 15d;
        double expandedFakeViewHeight => Height * 0.7d;
        double expandedFakeViewWidth => Width - 60d;
        List<Frame> upperImages;
        List<Frame> bottomImages;
        bool isExpanded = false;

        public double SelectedViewWidth { get; set; }
        public double SelectedViewHeight { get; set; }
        public double DefaultViewWidth { get => SelectedViewWidth * 0.86d; }
        public double DefaultViewHeight { get => SelectedViewHeight * 0.86d; }
        public PathGeometry ImageClip { get; set; }

        public string BlackPantherImage { get; set; }
        public string BlackWidowImage { get; set; }
        public string CaptainAmericaImage { get; set; }
        public string NebulaImage { get; set; }
        public string ScarletWitchImage { get; set; }
        public string StarlordImage { get; set; }

        public HomePage()
        {
            BlackPantherImage = "blackpanther.jpg";
            BlackWidowImage = "blackwidow.jpg";
            CaptainAmericaImage = "captainamerica.jpg";
            NebulaImage = "nebula.jpg";
            ScarletWitchImage = "scarletwitch.jpg";
            StarlordImage = "starlord.jpg";

            InitializeComponent();

            upperImages = new List<Frame> { upLeftImage, upCenterImage, upRightImage };
            bottomImages = new List<Frame> { downLeftImage, downCenterImage, downRightImage };

            BindingContext = ((App)App.Current).ServiceProvider.GetRequiredService<IHomePageViewModel>();

            accelerometerService = ((App)App.Current).ServiceProvider.GetRequiredService<IAccelerometerService>();

            SizeChanged += HomePageSizeChanged;
        }

        private void HomePageSizeChanged(object sender, EventArgs e)
        {
            SelectedViewWidth = Width - 120;
            SelectedViewHeight = SelectedViewWidth * 1.5d;
            coverFlowView.PositionShiftValue = defaultPositionShiftValue = (((Width - SelectedViewWidth) / 2d) * 0.86d) + 40;

            fakeView.WidthRequest = SelectedViewWidth;
            fakeView.HeightRequest = SelectedViewHeight;

            movieNameLabel.TranslationY = shrinkedNameLabelY;
            movieInfoFrame.TranslationY = shrinkedMovieInfoFrameY;

            SetPositionsOfImages();
            UpdateImageClip();

            OnPropertyChanged(nameof(SelectedViewWidth));
            OnPropertyChanged(nameof(SelectedViewHeight));
            OnPropertyChanged(nameof(DefaultViewWidth));
            OnPropertyChanged(nameof(DefaultViewHeight));
        }

        private void SetPositionsOfImages()
        {
            double upperY = -(SelectedViewHeight / 2d) - (DefaultViewHeight / 2d) - spacing;
            double bottomY = -upperY;
            double leftX = - DefaultViewWidth - ((Width / 2d) - (DefaultViewWidth / 2d) - (40 * 0.86d));
            double rightX = -leftX;

            upperImageDefaultY = upperY;
            bottomImageDefaultY = bottomY;

            upLeftImage.TranslateTo(leftX, upperY, 0);
            upCenterImage.TranslateTo(upCenterImage.TranslationX, upperY, 0);
            upRightImage.TranslateTo(rightX, upperY, 0);
            downLeftImage.TranslateTo(leftX, bottomY, 0);
            downCenterImage.TranslateTo(upCenterImage.TranslationX, bottomY, 0);
            downRightImage.TranslateTo(rightX, bottomY, 0);
        }

        private void UpdateImageClip()
        {
            double cornerRadius = 8;
            
            ImageClip = new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    new PathFigure
                    {
                        IsClosed = true, IsFilled = true, StartPoint = new Point(DefaultViewWidth - cornerRadius, 0),
                        Segments = new PathSegmentCollection
                        {
                            new QuadraticBezierSegment(new Point(DefaultViewWidth, 0), new Point(DefaultViewWidth, cornerRadius)),
                            new LineSegment(new Point(DefaultViewWidth, DefaultViewHeight - cornerRadius)),
                            new QuadraticBezierSegment(new Point(DefaultViewWidth, DefaultViewHeight), new Point(DefaultViewWidth - cornerRadius, DefaultViewHeight)),
                            new LineSegment(new Point(cornerRadius, DefaultViewHeight)),
                            new QuadraticBezierSegment(new Point(0, DefaultViewHeight), new Point(0, DefaultViewHeight - cornerRadius)),
                            new LineSegment(new Point(0, cornerRadius)),
                            new QuadraticBezierSegment(new Point(0, 0), new Point(cornerRadius, 0))
                        }
                    }
                }
            };

            upLeftImage.Clip = ImageClip;

            OnPropertyChanged(nameof(ImageClip));
        }

        private async void MovieViewTapped(object sender, EventArgs e)
        {
            if(sender is MovieView view && view == coverFlowView.CurrentView)
            {
                if (!isExpanded)
                {
                    if (fakeView.BindingContext != coverFlowView.SelectedItem)
                        fakeView.BindingContext = coverFlowView.SelectedItem;
                    movieNameLabel.Text = ((Movie)coverFlowView.SelectedItem).Name;

                    isExpanded = true;

                    fakeView.IsVisible = true;
                    await AnimateEverythingOutAsync();
                    fakeView.InputTransparent = false;
                    accelerometerService.TranslationChanged += AccelerometerTranslationChanged;
                }
            }
        }

        private async void FakeMovieViewTapped(object sender, EventArgs e)
        {
            if(isExpanded)
            {
                isExpanded = false;

                fakeView.InputTransparent = true;
                await AnimateEverythingInAsync();
                fakeView.IsVisible = false;

                accelerometerService.TranslationChanged -= AccelerometerTranslationChanged;
            }
        }
        
        private async Task AnimateEverythingOutAsync()
        {
            List<Task> animations = new List<Task>();

            foreach (var image in upperImages)
            {
                animations.Add(image.TranslateTo(image.TranslationX, -(Height / 2d) - DefaultViewHeight));
            }

            foreach (var image in bottomImages)
            {
                animations.Add(image.TranslateTo(image.TranslationX, (Height / 2d) + DefaultViewHeight));
            }

            animations.Add(ExpandFakeViewAsync());
            animations.Add(fakeView.Expand());

            Animation animation = new Animation(v => coverFlowView.PositionShiftValue = v , defaultPositionShiftValue, 0);
            animation.Commit(this, "PositionShiftValueAnimation");

            await Task.WhenAll(animations);
        }

        private async Task AnimateEverythingInAsync()
        {
            List<Task> animations = new List<Task>();

            foreach (var image in upperImages)
            {
                animations.Add(image.TranslateTo(image.TranslationX, upperImageDefaultY));
            }

            foreach (var image in bottomImages)
            {
                animations.Add(image.TranslateTo(image.TranslationX, bottomImageDefaultY));
            }

            animations.Add(ShrinkFakeViewAsync());
            animations.Add(fakeView.Shrink());

            Animation animation = new Animation(v => coverFlowView.PositionShiftValue = v, 0, defaultPositionShiftValue);
            animation.Commit(this, "PositionShiftValueAnimation");

            await Task.WhenAll(animations);
        }

        private async Task ExpandFakeViewAsync()
        {
            uint animationTime = 250;

            await Task.Delay(100);

            Animation widthAnimation = new Animation(v => fakeView.WidthRequest = v, SelectedViewWidth, expandedFakeViewWidth, Easing.SpringOut);
            Animation heightAnimation = new Animation(v => fakeView.HeightRequest = v, SelectedViewHeight, expandedFakeViewHeight, Easing.SpringOut);

            Animation animation = new Animation();

            animation.Add(0, 1, widthAnimation);
            animation.Add(0, 1, heightAnimation);

            animation.Commit(this, "SizeAnimation", length: animationTime);

            movieInfoFrame.IsVisible = true;

            await Task.WhenAll(
                movieNameLabel.TranslateTo(movieNameLabel.TranslationX, expandedNameLabelY, animationTime),
                movieNameLabel.ScaleTo(1, animationTime, Easing.SpringOut),
                movieNameLabel.FadeTo(1, animationTime),
                movieInfoFrame.TranslateTo(movieInfoFrame.TranslationX, expandedMovieInfoFrameY, animationTime)
                );
        }

        private async Task ShrinkFakeViewAsync()
        {
            uint animationTime = 250;

            await Task.Delay(100);

            Animation widthAnimation = new Animation(v => fakeView.WidthRequest = v, expandedFakeViewWidth, SelectedViewWidth, Easing.SpringOut);
            Animation heightAnimation = new Animation(v => fakeView.HeightRequest = v, expandedFakeViewHeight, SelectedViewHeight, Easing.SpringOut);

            Animation animation = new Animation();

            animation.Add(0, 1, widthAnimation);
            animation.Add(0, 1, heightAnimation);

            animation.Commit(this, "SizeAnimation", length: animationTime);

            await Task.WhenAll(
                movieNameLabel.TranslateTo(movieNameLabel.TranslationX, shrinkedNameLabelY, animationTime),
                movieNameLabel.ScaleTo(0, animationTime, Easing.SpringIn),
                movieNameLabel.FadeTo(0, animationTime),
                movieInfoFrame.TranslateTo(movieInfoFrame.TranslationX, shrinkedMovieInfoFrameY, animationTime)
                );

            movieInfoFrame.IsVisible = false;
        }

        private void AccelerometerTranslationChanged(double x, double y)
        {
            double translationX = x * 1.8d;

            var offset = (Width - movieInfoFrame.Width) / 2d;

            if (translationX < 0 && translationX < -offset)
                translationX = -offset + 4;

            if (translationX > 0 && translationX > offset)
                translationX = offset - 4;

            Task.WhenAll(
                movieInfoFrame.TranslateTo(translationX, movieInfoFrame.TranslationY, 150)
                );
        }
    }
}