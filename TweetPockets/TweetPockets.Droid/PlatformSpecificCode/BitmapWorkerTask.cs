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
using TweetPockets.Droid.PlatformSpecificCode.Cache;
using Xamarin.Forms;
using Object = Java.Lang.Object;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class BitmapWorkerTask : AsyncTask<string, Java.Lang.Void, Bitmap>
    {
        private readonly WeakReference<ImageView> _imageViewReference;
        private BitmapCache _cache = new BitmapCache();

        public BitmapWorkerTask(ImageView imageView)
        {
            _imageViewReference = new WeakReference<ImageView>(imageView);
        }

        protected override Bitmap RunInBackground(params string[] native_parms)
        {
            var imageUrl = (string) native_parms[0];
            return _cache.Get(imageUrl);
            
        }

        protected override void OnPostExecute(Bitmap bitmap)
        {
            if (IsCancelled)
            {
                bitmap = null;
            }
            else
            {
                if (_imageViewReference != null && bitmap != null)
                {
                    ImageView imageView;
                    if (_imageViewReference.TryGetTarget(out imageView))
                    {
                        imageView.SetImageBitmap(bitmap);
                    }
                }
            }
        }
    }
}