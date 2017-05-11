using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Managers;
using TweetPockets.Managers.Notifications;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;
using Xamarin.Auth;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int StatusTextLength = 15;
        private readonly StatusLoadingManager _loadingManager;
        private readonly BookmarkPersistingManager _bookmarkPersistingManager;
        private MenuItemViewModel _selectedItem;

        public MainViewModel()
        {
            var persistingManager = new StatusPersistingManager();
            _bookmarkPersistingManager = new BookmarkPersistingManager(persistingManager);
            _loadingManager = new StatusLoadingManager(_bookmarkPersistingManager);
            //Info = new InfoViewModel(_loadingManager);
            Timeline = new TimelineViewModel(this, _loadingManager, persistingManager, _bookmarkPersistingManager);
            //BookmarkList = new BookmarkListViewModel(this, _bookmarkPersistingManager);
            //Activity = new ActivityViewModel(this, new EventPersistingManager());
            //SelectedItem = Timeline;

            //MessagingCenter.Subscribe<MainViewModel, StatusViewModel>(this, "ChangeBookmarkState",
            //    (s, status) => OnChangeBookmarkState(status));
            //MessagingCenter.Subscribe<MainViewModel, BookmarkViewModel>(this, "RemoveBookmark",
            //    (s, status) => OnRemoveBookmark(status));
        }

        //public InfoViewModel Info { get; set; }

        public TimelineViewModel Timeline { get; set; }

        //public BookmarkListViewModel BookmarkList { get; set; }

        //public ActivityViewModel Activity { get; set; }

        //public MenuItemViewModel SelectedItem
        //{
        //    get { return _selectedItem; }
        //    set
        //    {
        //        if (_selectedItem != null)
        //        {
        //            _selectedItem.IsSelected = false;
        //        }

        //        _selectedItem = value;
        //        _selectedItem.IsSelected = true;
        //    }
        //}

        public async Task InitAsync(Account account)
        {
            await _loadingManager.Init(account);

            //Info.InitAsync();
            await Timeline.InitAsync();
            //BookmarkList.InitAsync();
        }

        //private void OnChangeBookmarkState(StatusViewModel status)
        //{
        //    string pattern = null;

        //    if (!status.IsBookmarked)
        //    {
        //        var newBookmark = _bookmarkPersistingManager.CreateBookmark(status);
        //        BookmarkList.Bookmarks.Add(newBookmark);

        //        pattern = AppResources.AddedToBookmarksNotificationPattern;
        //    }
        //    else
        //    {
        //        var oldBookmark = _bookmarkPersistingManager.RemoveBookmarkWithStatus(status);
        //        BookmarkList.Bookmarks.Remove(oldBookmark);

        //        pattern = AppResources.RemovedFromBookmarksNotificationPattern;
        //    }

        //    var manager = DependencyService.Get<IToastNotificationController>();
        //    manager.ShowToast(String.Format(pattern, GetShortStatusText(status.Text)));
        //}

        //private void OnRemoveBookmark(BookmarkViewModel bookmark)
        //{
        //    _bookmarkPersistingManager.RemoveBookmark(bookmark);
        //    BookmarkList.Bookmarks.Remove(bookmark);

        //    var relatedStatus = Timeline.Timeline.GetById(bookmark.Id);
        //    if (relatedStatus != null)
        //    {
        //        relatedStatus.IsBookmarked = false;
        //    }

        //    var manager = DependencyService.Get<IToastNotificationController>();
        //    manager.ShowToast(String.Format(AppResources.RemovedFromBookmarksNotificationPattern, GetShortStatusText(bookmark.Text)));
        //}

        //private string GetShortStatusText(string text)
        //{
        //    return text.Length > StatusTextLength ? text.Substring(0, StatusTextLength - 3) + "..." : text;
        //}
    }
}
