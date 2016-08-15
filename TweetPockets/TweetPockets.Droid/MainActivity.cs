using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using TweetPockets.Interfaces;
using Xamarin.Auth;

using Xamarin.Forms;

namespace TweetPockets.Droid
{
    [Activity(Label = "Tweet Pockets", Icon = "@drawable/icon", MainLauncher = true,
        Theme = "@style/Theme.MaterialExtended",
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Instance = this;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var app = new App();
            LoadApplication(app);

            if (!app.IsLoggedIn)
            {
                var auth = new OAuth1Authenticator(
                    consumerKey: Keys.TwitterConsumerKey,
                    consumerSecret: Keys.TwitterConsumerSecret,
                    requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
                    authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
                    accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
                    callbackUrl: new Uri("x-oauthflow-twitter://callback")
                    );
                auth.AllowCancel = true;
                auth.Completed += (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        MessagingCenter.Send<App, Xamarin.Auth.Account>(App.Instance, "LoggedIn", eventArgs.Account);
                    }
                };

                StartActivity(auth.GetUI(this));
            }

            InitScreenSize();
        }

        private void InitScreenSize()
        {
            DisplayMetrics displayMetrics = this.Resources.DisplayMetrics;
            Height = displayMetrics.HeightPixels / displayMetrics.Density;
            Width = displayMetrics.WidthPixels / displayMetrics.Density;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Drawable.menu, menu);
            return true;
        }
    }
}

