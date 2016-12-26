using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.ViewModels.Collections
{
    public class Timeline : BatchedObservableCollection<ITimelineEntity>
    {
        public override bool HasMoreItems
        {
            get { return Count > 0; }
        }
    }
}
