using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Managers;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Collections;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class BookmarkListViewModel : MenuItemViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly BookmarkPersistingManager _persistingManager;

        public BookmarkListViewModel(MainViewModel mainViewModel, BookmarkPersistingManager persistingManager)
            : base(AppResources.BookmarksMenuItem, "ic_book_black_24dp.png", "ic_book_green_24dp.png")
        {
            _mainViewModel = mainViewModel;
            _persistingManager = persistingManager;
            Bookmarks = new Bookmarks();
            RemoveBookmarkCommand = new Command(RemoveBookmark);
        }

        public BatchedObservableCollection<ITimelineEntity> Bookmarks { get; private set; }

        public ICommand RemoveBookmarkCommand { get; set; }

        public void InitAsync()
        {
            Bookmarks.AddRange(_persistingManager.GetBookmarks());
        }

        private void RemoveBookmark(object obj)
        {
            var item = (BookmarkViewModel)obj;

            MessagingCenter.Send(_mainViewModel, "RemoveBookmark", item);
        }
    }
}
