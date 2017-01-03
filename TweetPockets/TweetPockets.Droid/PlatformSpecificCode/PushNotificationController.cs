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
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PushNotificationsController))]

namespace TweetPockets.Droid.PlatformSpecificCode
{
    public class PushNotificationsController : IPushNotificationsController
    {
        private IDictionary<UserStreamEventType, int> _icons = new Dictionary<UserStreamEventType, int>()
        {
            { UserStreamEventType.Favorite, Resource.Drawable.ic_favorite_red },
            { UserStreamEventType.Retweet, Resource.Drawable.ic_repeat_blue },
            { UserStreamEventType.Quoted, Resource.Drawable.ic_quote_white }
        };

        public void ShowNotification(UserStreamEventType eventType, string title, string text, string summary)
        {
            Context context = Forms.Context;

            Intent resultIntent = new Intent(context, typeof(MainActivity));
            //resultIntent.PutExtra("message", "Greetings from MainActivity!");
            PendingIntent resultPendingIntent =
                PendingIntent.GetActivity(
                context,
                0,
                resultIntent,
                PendingIntentFlags.OneShot
            );

            int icon;
            _icons.TryGetValue(eventType, out icon);
            Notification.Builder builder = new Notification.Builder(context)
                .SetAutoCancel(true)
                .SetContentTitle(title)
                .SetContentText(text)
                .SetTicker(summary)
                .SetDefaults(NotificationDefaults.All)
                .SetSmallIcon(icon != 0 ? icon : Resource.Drawable.icon)
                .SetContentIntent(resultPendingIntent)
                .SetCategory(Notification.CategorySocial);

            var notification = builder.Build();
            var notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager?.Notify(0, notification);
        }
    }
}