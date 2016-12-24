using System;
using System.Collections.Generic;
using TweetPockets.Interfaces.Entities;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Managers
{
    public class ItemsEventArgs : EventArgs
    {
        public IList<ITimelineEntity> Items { get; set; }
    }
}