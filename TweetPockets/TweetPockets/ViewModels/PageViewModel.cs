using System.Threading.Tasks;

namespace TweetPockets.ViewModels
{
    public abstract class PageViewModel : ViewModelBase
    {
        public virtual async Task InitAsync()
        {
        }
    }
}
