using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Object = Java.Lang.Object;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class BitmapWorkerTask : AsyncTask<string, Java.Lang.Void, Bitmap>
    {
        private readonly WeakReference<ImageView> imageViewReference;
        private string _data;

        public BitmapWorkerTask(ImageView imageView)
        {
            imageViewReference = new WeakReference<ImageView>(imageView);
        }

        protected override Bitmap RunInBackground(params string[] native_parms)
        {
            _data = (string) native_parms[0];
            var main = MainActivity.Instance;
            return BitmapUtils.GetImageBitmapFromUrl(_data, main.Width, main.Height);
        }

        protected override void OnPostExecute(Bitmap bitmap)
        {
            if (IsCancelled)
            {
                bitmap = null;
            }
            else
            {
                if (imageViewReference != null && bitmap != null)
                {
                    ImageView imageView;
                    if (imageViewReference.TryGetTarget(out imageView))
                    {
                        imageView.SetImageBitmap(bitmap);
                    }
                }
            }
        }
    }
}