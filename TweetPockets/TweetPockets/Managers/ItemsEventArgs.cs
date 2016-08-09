using System;
using System.Collections.Generic;
using TweetPockets.ViewModels;

namespace TweetPockets.Managers
{
    public class ItemsEventArgs : EventArgs
    {
        public IList<StatusViewModel> Items { get; set; }
    }
}