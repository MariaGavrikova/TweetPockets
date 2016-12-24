using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly BookmarkPersistingManager _persistingManager;

        public BookmarkListViewModel(BookmarkPersistingManager persistingManager)
            : base(AppResources.BookmarksMenuItem, "ic_book_black_24dp.png")
        {
            _persistingManager = persistingManager;
            Bookmarks = new Bookmarks();
        }

        public BatchedObservableCollection<ITimelineEntity> Bookmarks { get; private set; }

        public void InitAsync()
        {
            Bookmarks.AddRange(_persistingManager.GetBookmarks());
        }
    }
}
