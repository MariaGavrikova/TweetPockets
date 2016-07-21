using System;
using Android.Graphics;
using Java.Lang;
using Xamarin.Forms;

namespace TweetPockets.Droid.PlatformSpecificCode.Cache
{
    class BitmapCache
    {
        private static readonly MemoryCache _memoryCache;
        private static readonly DiskCache _diskCache;

        static BitmapCache()
        {
            var maxSize = (int)(Runtime.GetRuntime().MaxMemory() / 1024 / 8);
            _memoryCache = new MemoryCache(maxSize);
            _diskCache = DiskCache.CreateCache(Forms.Context, "DiskCache");
        }

        public Bitmap Get(string imageUrl)
        {
            Bitmap result = (Bitmap) _memoryCache.Get(imageUrl);
            if (result == null)
            {
                if (!_diskCache.TryGet(imageUrl, out result))
                {
                    var main = MainActivity.Instance;
                    result = BitmapUtils.GetImageBitmapFromUrl(imageUrl, main.Width, main.Height);
                    _memoryCache.Put(imageUrl, result);
                    _diskCache.AddOrUpdate(imageUrl, result, TimeSpan.FromDays(3));
                }
            }
            return result;
        }
    }
}