﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Managers.Authorization;
using TweetPockets.Resources;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.Managers.Notifications
{
    public class FavoritesNotificationsManager : EventPersistingManager, IEventManager
    {
        public void Process(EventViewModel twitterEvent)
        {
            var initiator = twitterEvent.InitiatorId;
            if (initiator != AuthorizationManager.Instance.CurrentUserDetails.TwitterId)
            {
                if (twitterEvent.EventType == UserStreamEventType.Favorite)
                {
                    Save(twitterEvent);

                    var pushController = DependencyService.Get<IPushNotificationsController>();
                    pushController.ShowNotification(
                        twitterEvent.EventType,
                        twitterEvent.InitiatorName,
                        String.Format(AppResources.NotificationFullFavorited, twitterEvent.Text),
                        String.Format(AppResources.NotificationFavorited, twitterEvent.InitiatorName, twitterEvent.Text));
                }
                else if (twitterEvent.EventType == UserStreamEventType.Unfavorite)
                {
                    Remove(twitterEvent);
                }
            }
        }
    }
}
