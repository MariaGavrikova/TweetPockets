using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;
using TweetPockets.Interfaces;
using TweetPockets.Interfaces.Entities;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    public class BookmarkPersistingManager
    {
        private readonly StatusPersistingManager _statusPersistingManager;
        private const string DatabaseFileName = "data.db";
        private readonly Database _db;

        public BookmarkPersistingManager(StatusPersistingManager statusPersistingManager)
        {
            _statusPersistingManager = statusPersistingManager;
            var platformFactory = DependencyService.Get<ISQLitePlatformFactory>();
            _db = new Database(platformFactory.CreatePlatform(), platformFactory.CreateDatabaseFile(DatabaseFileName));
        }

        public BookmarkViewModel CreateBookmark(StatusViewModel status)
        {
            status.IsBookmarked = true;
            _statusPersistingManager.Save(status);
            var bookmark = new BookmarkViewModel(status);
            _db.Insert(bookmark);
            return bookmark;
        }

        public BookmarkViewModel RemoveBookmarkWithStatus(StatusViewModel status)
        {
            status.IsBookmarked = false;
            _statusPersistingManager.Save(status);

            var bookmark = _db.Find<BookmarkViewModel>(status.Id);
            _db.Delete(bookmark);
            return bookmark;
        }

        public void RemoveBookmark(BookmarkViewModel bookmark)
        {
            _db.Delete(bookmark);
            var status = _db.Find<StatusViewModel>(bookmark.Id);
            if (status != null)
            {
                status.IsBookmarked = false;
                _statusPersistingManager.Save(status);
            }
        }

        public IEnumerable<ITimelineEntity> GetBookmarks()
        {
            return _db.Table<BookmarkViewModel>();
        }

        public bool IsStatusBookmarked(long id)
        {
            return _db.Find<BookmarkViewModel>(id) != null;
        }
    }
}
