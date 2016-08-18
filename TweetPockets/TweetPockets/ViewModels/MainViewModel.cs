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
        private int StatusTextLength = 15;
        private readonly StatusLoadingManager _loadingManager;

        public MainViewModel()
        {
            _loadingManager = new StatusLoadingManager();
            Info = new InfoViewModel(_loadingManager);
            Timeline = new TimelineViewModel(this, _loadingManager);
            BookmarkList = new BookmarkListViewModel(this);

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
        }

        private void OnAddBookmark(StatusViewModel status)
        {
            Timeline.Timeline.Remove(status);
            BookmarkList.Bookmarks.Add(status);

            var manager = DependencyService.Get<INotificationController>();
            manager.ShowToast(String.Format(AppResources.AddedToBookmarksNotificationPattern, GetShortStatusText(status.Text)));
        }

        private string GetShortStatusText(string text)
        {
            return text.Length > StatusTextLength ? text.Substring(0, StatusTextLength - 3) + "..." : text;
        }
    }
}
