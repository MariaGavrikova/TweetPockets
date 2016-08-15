using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Managers;
using TweetPockets.Resources;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int StatusTextLength = 15;
        private StatusLoadingManager _loadingManager;
        private string _avatarUrl;
        private string _screenName;
        private string _name;

        public MainViewModel()
        {
            _loadingManager = new StatusLoadingManager();
            Timeline = new TimelineViewModel(this, _loadingManager);
            BookmarkList = new BookmarkListViewModel(this);

            MessagingCenter.Subscribe<MainViewModel, StatusViewModel>(this, "AddBookmark",
                (s, status) => OnAddBookmark(status));
        }

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set
            {
                _avatarUrl = value;
                OnPropertyChanged();
            }
        }

        public string ScreenName
        {
            get { return _screenName; }
            set
            {
                _screenName = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public TimelineViewModel Timeline { get; set; }

        public BookmarkListViewModel BookmarkList { get; set; }

        public async Task InitAsync(UserDetails user)
        {
            await Timeline.InitAsync(user);
            var userInfo = await _loadingManager.GetUserInfo(user.ScreenName);
            AvatarUrl = userInfo.ProfileImageUrl;
            ScreenName = userInfo.ScreenName;
            Name = userInfo.Name;
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
