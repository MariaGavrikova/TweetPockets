using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using TweetPockets;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using ListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]

namespace TweetPockets.Droid.Renderers
{
    public class CircleImageRenderer : ViewRenderer<CircleImage, RoundedImageView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CircleImage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var image = new RoundedImageView(Forms.Context);
                SetNativeControl(image);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CircleImage.SourceProperty.PropertyName)
            {
                LoadImageAsync();
            }
        }

        private void LoadImageAsync()
        {
            var worker = new BitmapWorkerTask(Control);
            worker.Execute(Element.Source);
        }
    }
}