using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using Java.Util;
using TweetPockets;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Animation = Xamarin.Forms.Animation;
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

            var newElement = e.NewElement;
            if (newElement != null)
            {
                var parent = new LinearLayout(Forms.Context);
                var button = (Forms.Context as Activity).LayoutInflater.Inflate(Resource.Layout.TweetButton, parent);
                button.Click += ClickedHandler;
                SetNativeControl(parent);
            }
        }

        private void HideButton()
        {
            Control.Visibility = ViewStates.Gone;

            
        }

        private void ClickedHandler(object sender, EventArgs e)
        {
            //var animation = AnimationUtils.LoadAnimation(Forms.Context, Resource.Animation.design_fab_out);
            //animation.AnimationEnd += (s, args) => HideButton();
            //Control.StartAnimation(animation);
            //Control.Clickable = false;

            var command = Element.Command;
            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }
    }
}