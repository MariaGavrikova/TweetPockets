using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Managers;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int StatusTextLength = 15;
        private readonly StatusLoadingManager _loadingManager;
        private readonly BookmarkPersistingManager _bookmarkPersistingManager;

        public MainViewModel()
        {
            var persistingManager = new StatusPersistingManager();
            _bookmarkPersistingManager = new BookmarkPersistingManager(persistingManager);
            _loadingManager = new StatusLoadingManager(_bookmarkPersistingManager);
            Info = new InfoViewModel(_loadingManager);
            Timeline = new TimelineViewModel(this, _loadingManager, persistingManager, _bookmarkPersistingManager);
            BookmarkList = new BookmarkListViewModel(_bookmarkPersistingManager);

            MostImportantItems = new List<MenuItemViewModel>()
            {
                Timeline,
                BookmarkList
            };

            MessagingCenter.Subscribe<MainViewModel, StatusViewModel>(this, "AddBookmark",
                (s, status) => OnAddBookmark(status));
        }

        public InfoViewModel Info { get; set; }

        public IList<MenuItemViewModel> MostImportantItems { get; private set; }

        public TimelineViewModel Timeline { get; set; }

        public BookmarkListViewModel BookmarkList { get; set; }

        public async Task InitAsync(UserDetails user)
        {
            await _loadingManager.Init(user);

            Info.InitAsync(user);
            Timeline.InitAsync();
            BookmarkList.InitAsync();
        }

        private void OnAddBookmark(StatusViewModel status)
        {
            if (!status.IsBookmarked)
            {
                Timeline.Timeline.Remove(status);
                
                var newBookmark = _bookmarkPersistingManager.CreateBookmark(status);
                BookmarkList.Bookmarks.Add(newBookmark);

                var manager = DependencyService.Get<INotificationController>();
                manager.ShowToast(String.Format(AppResources.AddedToBookmarksNotificationPattern, GetShortStatusText(status.Text)));
            }
            else
            {
                var oldBookmark = _bookmarkPersistingManager.RemoveBookmark(status);
                BookmarkList.Bookmarks.Remove(oldBookmark);
            }
        }

        private string GetShortStatusText(string text)
        {
            return text.Length > StatusTextLength ? text.Substring(0, StatusTextLength - 3) + "..." : text;
        }
    }
}
