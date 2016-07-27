﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Managers;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int StatusTextLength = 15;

        public MainViewModel()
        {
            var persistingManager = new StatusPersistingManager();
            var loadingManager = new StatusLoadingManager(persistingManager);
            Timeline = new TimelineViewModel(this, persistingManager, loadingManager);
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
            BookmarkList.Bookmarks.Add(status);

            var manager = DependencyService.Get<INotificationController>();
            manager.ShowToast(String.Format("\"{0}\" added to Bookmarks", GetShortStatusText(status.Text)));
        }

        private string GetShortStatusText(string text)
        {
            return text.Length > StatusTextLength ? text.Substring(0, StatusTextLength - 3) + "..." : text;
        }
    }
}
