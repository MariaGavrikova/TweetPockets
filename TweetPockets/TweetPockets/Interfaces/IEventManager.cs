using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Interfaces
{
    public interface IEventManager
    {
        void Process(EventViewModel twitterEvent);
    }
}