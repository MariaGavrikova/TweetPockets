using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using TweetPockets.Interfaces.Entities;

namespace TweetPockets.ViewModels.Entities
{
    public class ResourceUrlViewModel : ViewModelBase, IMediaEntity
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(StatusViewModel))]
        public long StatusId { get; set; }

        public string Url { get; set; }
    }
}
