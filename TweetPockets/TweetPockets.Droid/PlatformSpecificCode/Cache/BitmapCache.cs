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
        private readonly TimeSpan _diskCacheLifeTime = TimeSpan.FromDays(3);

        private const int CacheSize = 10 * 1024 * 1024; // 4MiB
        
        static BitmapCache()
        {
            _memoryCache = new MemoryCache(CacheSize);
            _diskCache = DiskCache.CreateCache(Forms.Context, "DiskCache");
        }

        public Bitmap Get(string key)
        {
            Bitmap bitmap = null;

            if (!string.IsNullOrEmpty(key))
            {
                bitmap = (Bitmap)_memoryCache.Get(key);

                if (bitmap == null)
                {
                    if (_diskCache.TryGet(key, out bitmap))
                    {
                        _memoryCache.Put(key, bitmap);
                    }
                    else
                    {
                        var main = MainActivity.Instance;
                        bitmap = BitmapUtils.GetImageBitmapFromUrl(key, main.Width, main.Height);
                        if (bitmap != null)
                        {
                            _memoryCache.Put(key, bitmap);
                            _diskCache.AddOrUpdate(key, bitmap, _diskCacheLifeTime);
                        }
                    }
                }
            }

            return bitmap;
        }
    }
}