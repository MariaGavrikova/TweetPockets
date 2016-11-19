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
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;

[assembly: ExportRenderer(typeof(MultilineTextField), typeof(MultilineTextFieldRenderer))]

namespace TweetPockets.Droid.Renderers
{
    public class MultilineTextFieldRenderer : ViewRenderer<MultilineTextField, Android.Views.View>
    {
        private const int MaxLenght = 120;

        protected override void OnElementChanged(ElementChangedEventArgs<MultilineTextField> e)
        {
            base.OnElementChanged(e);

            var newElement = e.NewElement;
            if (newElement != null)
            {
                var parent = new LinearLayout(Forms.Context);
                var view = (Forms.Context as Activity).LayoutInflater.Inflate(Resource.Layout.MultilineTextField, parent);
                var field = view.FindViewById<EditText>(Resource.Id.InputField);
                var currentLenght = view.FindViewById<TextView>(Resource.Id.CurrentLenght);
                var counterDelimiter = view.FindViewById<TextView>(Resource.Id.CounterDelimiter);
                var totalLenght = view.FindViewById<TextView>(Resource.Id.TotalLenght);
                totalLenght.Text = MaxLenght.ToString();
                field.TextChanged += (s, args) =>
                {
                    var enumerable = args.Text;
                    Element.SetValue(MultilineTextField.TextProperty, enumerable);
                    var length = args.AfterCount + args.Start;
                    currentLenght.Text = length.ToString();

                    var color = Color.Black;
                    if (length > MaxLenght)
                    {
                        color = Color.Red;
                    }
                    
                    currentLenght.SetTextColor(color);
                    counterDelimiter.SetTextColor(color);
                    totalLenght.SetTextColor(color);
                };
                SetNativeControl(parent);
            }
        }
    }
}