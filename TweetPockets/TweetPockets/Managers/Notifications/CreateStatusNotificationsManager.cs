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
        private IDictionary<UserStreamEventType, string> _titles = new Dictionary<UserStreamEventType, string>()
        {
            { UserStreamEventType.Retweet, AppResources.NotificationFullRetweeted },
            { UserStreamEventType.Quoted, AppResources.NotificationFullQuoted },
            { UserStreamEventType.Replied, AppResources.NotificationFullReplied }
        };

        private IDictionary<UserStreamEventType, string> _summaries = new Dictionary<UserStreamEventType, string>()
        {
            { UserStreamEventType.Retweet, AppResources.NotificationRetweeted },
            { UserStreamEventType.Quoted, AppResources.NotificationQuoted },
            { UserStreamEventType.Replied, AppResources.NotificationReplied }
        };

        public void Process(EventViewModel twitterEvent)
        {
            var initiator = twitterEvent.InitiatorId;
            var eventType = twitterEvent.EventType;
            var currentUserId = AuthorizationManager.Instance.CurrentUserDetails.TwitterId;
            if ((eventType == UserStreamEventType.Retweet || eventType == UserStreamEventType.Quoted || eventType == UserStreamEventType.Replied) &&
                twitterEvent.TargetUserId == currentUserId &&
                initiator != currentUserId)
            {
                Save(twitterEvent);

                var pushController = DependencyService.Get<IPushNotificationsController>();
                var title = String.Format(_titles[eventType], twitterEvent.Text);
                var summary = String.Format(_summaries[eventType], twitterEvent.InitiatorName, twitterEvent.Text);
                pushController.ShowNotification(
                    eventType,
                    twitterEvent.InitiatorName,
                    title,
                    summary);
            }
        }
    }
}
