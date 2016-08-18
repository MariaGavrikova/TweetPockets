using System;
using System.Collections.Generic;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Managers
{
    public class ItemsEventArgs : EventArgs
    {
        public IList<StatusViewModel> Items { get; set; }
    }
}