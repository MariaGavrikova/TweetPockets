using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Timeline = new TimelineViewModel(this);
            BookmarkList = new BookmarkListViewModel(this);

            MessagingCenter.Subscribe<MainViewModel, StatusViewModel>(this, "AddBookmark",
                (s, status) => OnAddBookmark(status));
        }

        public TimelineViewModel Timeline { get; set; }

        public BookmarkListViewModel BookmarkList { get; set; }

        public async Task InitAsync(UserDetails user)
        {
            await Timeline.InitAsync(user);
        }

        private void OnAddBookmark(StatusViewModel status)
        {
            Timeline.Timeline.Remove(status);
            BookmarkList.Bookmarks.Insert(0, status);

            var manager = DependencyService.Get<INotificationController>();
            manager.ShowToast("Added to Bookmarks");
        }
    }
}
