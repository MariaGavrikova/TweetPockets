using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    public static class BitmapUtils
    {
        public static Bitmap GetImageBitmapFromUrl(string url, int width, int height)
        {
            Bitmap imageBitmap = null;
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length, options);
                    options.InSampleSize = CalculateInSampleSize(options, width, height);
                    options.InJustDecodeBounds = false;
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length, options);
                }
            }

            return imageBitmap;
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            int height = options.OutHeight;
            int width = options.OutWidth;
            double inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                if (width > height)
                {
                    inSampleSize = Math.Round((float) height/(float) reqHeight);
                }
                else
                {
                    inSampleSize = Math.Round((float) width/(float) reqWidth);
                }
            }
            return (int) inSampleSize;
        }
    }
}