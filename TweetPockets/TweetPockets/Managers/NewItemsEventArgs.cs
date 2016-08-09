using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.ViewModels;

namespace TweetPockets.Managers
{
    public class NewItemsEventArgs : ItemsEventArgs
    {
        public bool TooManyNewItems { get; set; }
    }
}
