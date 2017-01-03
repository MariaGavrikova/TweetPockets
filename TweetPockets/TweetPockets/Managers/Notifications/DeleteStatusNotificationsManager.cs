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
    public class DeleteStatusNotificationsManager : EventPersistingManager, IEventManager
    {
        public void Process(EventViewModel twitterEvent)
        {
            Remove(twitterEvent);
        }
    }
}
