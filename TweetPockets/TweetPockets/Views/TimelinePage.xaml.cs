using System;
using TweetPockets.ViewModels;
using Xamarin.Forms;

namespace TweetPockets.Views
{
    public partial class TimelinePage : ContentPage
    {
        public TimelinePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            TweetButton.FadeTo(1, 500);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            TweetButton.FadeTo(0, 500);
        }
    }
}
