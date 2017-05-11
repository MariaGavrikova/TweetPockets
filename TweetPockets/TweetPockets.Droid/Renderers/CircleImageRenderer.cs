using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
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
using TweetPockets.Droid.PlatformSpecificCode.Cache;
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
        private CancellationTokenSource _photoLoadingCancellation;
        private Task _photoLoadingTask;

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
                _photoLoadingCancellation?.Cancel();
                _photoLoadingTask?.Dispose();

                _photoLoadingCancellation = new CancellationTokenSource();
                _photoLoadingTask = Task
                    .Run(() => BitmapCache.Instance.Get(Element.Source), _photoLoadingCancellation.Token)
                    .ContinueWith(t => LoadImage(t, Control),
                        _photoLoadingCancellation.Token,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        protected void LoadImage(Task imageLoadingTask, ImageView image)
        {
            image.SetImageBitmap(((Task<Bitmap>)imageLoadingTask).Result);
        }
    }
}