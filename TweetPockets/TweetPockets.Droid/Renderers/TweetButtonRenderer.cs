using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using TweetPockets;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using ListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(TweetButton), typeof(TweetButtonRenderer))]

namespace TweetPockets.Droid.Renderers
{
    public class TweetButtonRenderer : ViewRenderer<TweetButton, Android.Views.View>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TweetButton> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var parent = new LinearLayout(Forms.Context);
                var button = (Forms.Context as Activity).LayoutInflater.Inflate(Resource.Layout.TweetButton, parent);
                SetNativeControl(parent);
            }
        }
    }
}