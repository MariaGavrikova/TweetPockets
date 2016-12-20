using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.ViewModels.Collections
{
    public class Bookmarks : BatchedObservableCollection<StatusViewModel>
    {
        public override bool HasMoreItems
        {
            get { return false; } 
        }
    }
}
