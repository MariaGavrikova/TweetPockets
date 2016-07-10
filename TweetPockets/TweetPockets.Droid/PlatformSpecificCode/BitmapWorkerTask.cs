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
        private WeakReference<ImageView> imageViewReference;
        private string _data;

        public BitmapWorkerTask(ImageView imageView)
        {
            imageViewReference = new WeakReference<ImageView>(imageView);
        }

        public static Bitmap DecodeSampledBitmapFromResource(Resources res, int resId, int reqWidth, int reqHeight)
        {

            // First decode with inJustDecodeBounds=true to check dimensions
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeResource(res, resId, options);

            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;
            return BitmapFactory.DecodeResource(res, resId, options);
        }

        public static int CalculateInSampleSize(
            BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {

                int halfHeight = height / 2;
                int halfWidth = width / 2;

                // Calculate the largest inSampleSize value that is a power of 2 and keeps both
                // height and width larger than the requested height and width.
                while ((halfHeight / inSampleSize) > reqHeight
                        && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;
        }

        protected override Bitmap RunInBackground(params string[] native_parms)
        {
            _data = (string) native_parms[0];
            return BitmapUtils.GetImageBitmapFromUrl(_data, 300, 300);
        }

        protected override void OnPostExecute(Bitmap bitmap)
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