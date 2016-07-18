using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationController))]

namespace TweetPockets.Droid.PlatformSpecificCode
{
    public class NotificationController : INotificationController
    {
        public void ShowToast(string message)
        {
            Context context = Forms.Context;
            Toast toast = Toast.MakeText(context, message, ToastLength.Short);
            toast.Show();
        }
    }
}