using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    public class RoundedImageView : ImageView
    {
        public RoundedImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public RoundedImageView(Context context) : base(context)
        {
        }

        public RoundedImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public RoundedImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public RoundedImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {

            Drawable drawable = this.Drawable;

            if (drawable == null)
            {
                return;
            }

            if (Width == 0 || Height == 0)
            {
                return;
            }

            Bitmap imageBitmap = null;
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop
                && drawable is VectorDrawable)
            {
                ((VectorDrawable)drawable).Draw(canvas);
                imageBitmap = Bitmap.CreateBitmap(canvas.Width, canvas.Height, Bitmap.Config.Argb8888);
                Canvas bitmapCanvas = new Canvas();
                bitmapCanvas.SetBitmap(imageBitmap);
                drawable.Draw(bitmapCanvas);
            }
            else
            {
                imageBitmap = ((BitmapDrawable) drawable).Bitmap;
            }

            Bitmap bitmapCopy = imageBitmap.Copy(Bitmap.Config.Argb8888, true);

            int w = Width, h = Height;

            Bitmap roundBitmap = GetCroppedBitmap(bitmapCopy, System.Math.Min(h, w));
            canvas.DrawBitmap(roundBitmap, 0, 0, null);
        }

        private static Bitmap GetCroppedBitmap(Bitmap bmp, int radius)
        {
            Bitmap sbmp;
            if (bmp.Width != radius || bmp.Height != radius)
            {
                sbmp = Bitmap.CreateScaledBitmap(bmp, radius, radius, false);
            }
            else
            {
                sbmp = bmp;
            }
            Bitmap output = Bitmap.CreateBitmap(sbmp.Width,
                    sbmp.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);

            uint color = 0xffa19774;
            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, sbmp.Width, sbmp.Height);

            paint.AntiAlias = true;
            paint.FilterBitmap = true;
            paint.Dither = true;
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = Color.ParseColor("#BAB399");
            canvas.DrawCircle(sbmp.Width / 2 + 0.7f, sbmp.Height / 2 + 0.7f,
                    sbmp.Width / 2 + 0.1f, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(sbmp, rect, rect, paint);
            return output;
        }
    }
}