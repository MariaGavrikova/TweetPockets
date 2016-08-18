using System;
using Android.Graphics;
using Android.Widget;
using TweetPockets.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace TweetPockets.Droid.Renderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var label = (TextView)Control;
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, GetFontResource(e.NewElement.FontFamily));
                label.Typeface = font;
            }
        }

        private static string GetFontResource(string fontFamily)
        {
            return String.Format("{0}.ttf", fontFamily);
        }
    }
}