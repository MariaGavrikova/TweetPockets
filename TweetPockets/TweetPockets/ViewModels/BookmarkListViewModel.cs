using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class BookmarkListViewModel : ViewModelBase
    {
        public BookmarkListViewModel(MainViewModel mainViewModel)
        {
            Bookmarks = new BatchedObservableCollection<StatusViewModel>();
        }

        public BatchedObservableCollection<StatusViewModel> Bookmarks { get; private set; }
    }
}
