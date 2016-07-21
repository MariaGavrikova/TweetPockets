using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Util;
using Android.Views;
using Android.Widget;
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
            //IntPtr classRef = JNIEnv.FindClass("android/graphics/Bitmap");
            //var getBytesMethodHandle = JNIEnv.GetMethodID(classRef, "getByteCount", "()I");
            //var byteCount = JNIEnv.CallIntMethod(value.Handle, getBytesMethodHandle);

            return ((Bitmap) value).ByteCount / 1024;
        }
    }
}