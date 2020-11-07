using Android.Content;
using Android.Views;
using MovieApp;
using MovieApp.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MovieView), typeof(MovieViewRenderer))]
namespace MovieApp.Droid
{
    public class MovieViewRenderer : ViewRenderer<MovieView, View>
    {
        public MovieViewRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            Element.Dispose();
            base.Dispose(disposing);
        }
    }
}