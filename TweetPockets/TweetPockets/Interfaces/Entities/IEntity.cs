using System.ComponentModel;

namespace TweetPockets.Interfaces.Entities
{
    public interface IEntity : INotifyPropertyChanged
    {
        long Id { get; set; }
    }
}