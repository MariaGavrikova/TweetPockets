using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class BookmarkListViewModel : MenuItemViewModel
    {
        public BookmarkListViewModel(MainViewModel mainViewModel)
            : base(AppResources.BookmarksMenuItem, "ic_book_black_24dp.png")
        {
            Bookmarks = new BatchedObservableCollection<StatusViewModel>();
        }

        public BatchedObservableCollection<StatusViewModel> Bookmarks { get; private set; }
    }
}
