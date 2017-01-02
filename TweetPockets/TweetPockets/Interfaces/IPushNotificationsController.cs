using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Interfaces
{
    public interface IPushNotificationsController
    {
        void ShowNotification(UserStreamEventType eventType, string title, string text, string summary);
    }
}