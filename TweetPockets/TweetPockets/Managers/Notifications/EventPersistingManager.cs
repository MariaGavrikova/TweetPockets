using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using TweetPockets.Interfaces;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.Managers.Notifications
{
    public class EventPersistingManager
    {
        private readonly Database _db;
        private const string DatabaseFileName = "data.db";
        private const int TotalItemsLimit = 100;

        public EventPersistingManager()
        {
            var platformFactory = DependencyService.Get<ISQLitePlatformFactory>();
            _db = new Database(platformFactory.CreatePlatform(), platformFactory.CreateDatabaseFile(DatabaseFileName));
        }

        private EventViewModel GetSavedCopy(EventViewModel twitterEvent)
        {
            return _db.Find<EventViewModel>(x =>
                x.InitiatorId == twitterEvent.InitiatorId &&
                x.StatusId == twitterEvent.StatusId &&
                (twitterEvent.EventType == UserStreamEventType.Unknown ||
                (twitterEvent.EventType== UserStreamEventType.Unfavorite && x.EventType == UserStreamEventType.Favorite) ||
                x.EventType == twitterEvent.EventType));
        }

        protected void Save(EventViewModel twitterEvent)
        {
            var savedEvent = GetSavedCopy(twitterEvent);
            if (savedEvent == null)
            {
                _db.Insert(twitterEvent);

                var eventsTable = _db.Table<EventViewModel>();
                var totalCount = eventsTable.Count();

                if (totalCount > TotalItemsLimit)
                {
                    _db.DeleteAll(eventsTable.OrderBy(x => x.CreatedAt).Take(totalCount - TotalItemsLimit), true);
                }
            }
        }

        protected void Remove(EventViewModel twitterEvent)
        {
            var savedEvent = GetSavedCopy(twitterEvent);
            if (savedEvent != null)
            {
                _db.Delete(savedEvent);
            }
        }

        public Task<TableQuery<EventViewModel>> GetEventsAsync()
        {
            return Task.FromResult(_db.Table<EventViewModel>());
        }
    }
}