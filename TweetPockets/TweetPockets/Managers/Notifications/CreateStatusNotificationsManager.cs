using System;
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
    public class CreateStatusNotificationsManager : EventPersistingManager, IEventManager
    {
        public void Process(EventViewModel twitterEvent)
        {
            var initiator = twitterEvent.InitiatorId;
            var eventType = twitterEvent.EventType;
            if ((eventType == UserStreamEventType.Retweet || eventType == UserStreamEventType.Quoted) &&
                initiator != AuthorizationManager.Instance.CurrentUserDetails.TwitterId)
            {
                Save(twitterEvent);

                var pushController = DependencyService.Get<IPushNotificationsController>();
                var title = 
                    String.Format(
                       eventType == UserStreamEventType.Retweet ? AppResources.NotificationFullRetweeted : AppResources.NotificationFullQuoted,
                       twitterEvent.Text);
                var summary = 
                    String.Format(
                        eventType == UserStreamEventType.Retweet ? AppResources.NotificationRetweeted : AppResources.NotificationQuoted, 
                        twitterEvent.InitiatorName,
                        twitterEvent.Text);
                pushController.ShowNotification(
                    eventType,
                    twitterEvent.InitiatorName, 
                    title,
                    summary);
            }
        }
    }
}
