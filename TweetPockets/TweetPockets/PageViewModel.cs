using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetPockets
{
    public abstract class PageViewModel : ViewModelBase
    {
        public virtual async Task InitAsync()
        {
        }
    }
}
