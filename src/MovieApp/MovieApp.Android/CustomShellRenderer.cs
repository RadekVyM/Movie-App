﻿using Android.Content;
using MovieApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(CustomShellRenderer))]
namespace MovieApp.Droid
{
    public class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context)
        {
        }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new CustomShellItemRenderer(this);
        }
    }
}