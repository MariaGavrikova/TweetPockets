using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Math = System.Math;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    public class AdjustableImageView : ImageView
    {
        private bool _adjustViewBounds;

        public AdjustableImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AdjustableImageView(Context context) : base(context)
        {
        }

        public AdjustableImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public AdjustableImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public AdjustableImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public override void SetAdjustViewBounds(bool adjustViewBounds)
        {
            _adjustViewBounds = adjustViewBounds;
            base.SetAdjustViewBounds(adjustViewBounds);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var drawable = this.Drawable;
            if (drawable == null)
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                return;
            }

            if (_adjustViewBounds)
            {
                int mDrawableWidth = drawable.IntrinsicWidth;
                int mDrawableHeight = drawable.IntrinsicHeight;
                int heightSize = View.MeasureSpec.GetSize(heightMeasureSpec);
                int widthSize = View.MeasureSpec.GetSize(widthMeasureSpec);
                var heightMode = View.MeasureSpec.GetMode(heightMeasureSpec);
                var widthMode = View.MeasureSpec.GetMode(widthMeasureSpec);

                if (heightMode == MeasureSpecMode.Exactly && widthMode != MeasureSpecMode.Exactly)
                {
                    // Fixed Height & Adjustable Width
                    int height = heightSize;
                    int width = height*mDrawableWidth/mDrawableHeight;
                    if (IsInScrollingContainer())
                        SetMeasuredDimension(width, height);
                    else
                        SetMeasuredDimension(Math.Min(width, widthSize), Math.Min(height, heightSize));
                }
                else if (widthMode == MeasureSpecMode.Exactly && heightMode != MeasureSpecMode.Exactly)
                {
                    // Fixed Width & Adjustable Height
                    int width = widthSize;
                    int height = width*mDrawableHeight/mDrawableWidth;
                    if (IsInScrollingContainer())
                    {
                        SetMeasuredDimension(width, height);
                    }
                    else
                    {
                        SetMeasuredDimension(Math.Min(width, widthSize), Math.Min(height, heightSize));
                    }
                }
                else
                {
                    base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                }
            }
            else
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }

        private bool IsInScrollingContainer()
        {
            var p = Parent;
            while (p != null && p is ViewGroup)
            {
                if (((ViewGroup) p).ShouldDelayChildPressedState())
                {
                    return true;
                }
                p = p.Parent;
            }
            return false;
        }
    }
}