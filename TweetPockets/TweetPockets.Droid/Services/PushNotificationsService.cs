using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LinqToTwitter;
using Org.Json;
using TweetPockets.Factories;
using TweetPockets.Interfaces;
using TweetPockets.Managers.Authorization;
using TweetPockets.Managers.Notifications;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;
using Xamarin.Auth;
using Account = Xamarin.Auth.Account;

namespace TweetPockets.Droid.Services
{
    [Service]
    public class PushNotificationsService : Service
    {
        private StreamContent _currentStream;

        private readonly IDictionary<string, IEventManager> _managers =
            new Dictionary<string, IEventManager>()
            {
                {"favorite", new FavoritesNotificationsManager()},
                {"unfavorite", new FavoritesNotificationsManager()},
                {"status", new CreateStatusNotificationsManager()},
                {"delete", new DeleteStatusNotificationsManager()}
            };

        private async Task StartUserStreamAsync()
        {
            Account account = null;
            while (account == null)
            {
                account = AccountStore.Create().FindAccountsForService("Twitter").FirstOrDefault();

                if (account == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(60));
                }
            }

            var context = await AuthorizationManager.Instance.GetContext(account);

            await
               (from strm in context.Streaming
                where strm.Type == StreamingType.User
                select strm)
               .StartAsync(async (s) => ProcessStream(s));
        }

        private void ProcessStream(StreamContent streamContent)
        {
            _currentStream = streamContent;

            try
            {
                string action = String.Empty;

                if (streamContent.EntityType == StreamEntityType.Event && streamContent.Entity is Event)
                {
                    var twitterEvent = (Event)streamContent.Entity;
                    action = twitterEvent.EventName;
                }

                if (streamContent.EntityType == StreamEntityType.Status && streamContent.Entity is Status)
                {
                    action = "status";
                }

                if (streamContent.EntityType == StreamEntityType.Delete && streamContent.Entity is Delete)
                {
                    action = "delete";
                }

                IEventManager manager;
                _managers.TryGetValue(action, out manager);
                manager?.Process(new EventViewModel(streamContent));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(StartUserStreamAsync);
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            _currentStream?.CloseStream();
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}