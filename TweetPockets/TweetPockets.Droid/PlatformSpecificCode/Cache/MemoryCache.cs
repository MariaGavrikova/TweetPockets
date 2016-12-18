using System;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V4.Util;
using Object = Java.Lang.Object;

namespace TweetPockets.Droid.PlatformSpecificCode.Cache
{
    class MemoryCache : LruCache
    {
        public MemoryCache(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MemoryCache(int maxSize) : base(maxSize)
        {
        }

        protected override int SizeOf(Object key, Object value)
        {
            return ((Bitmap)value).ByteCount / 1024;
        }
    }
}