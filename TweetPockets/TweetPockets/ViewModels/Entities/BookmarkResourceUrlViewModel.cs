using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using TweetPockets.Interfaces.Entities;

namespace TweetPockets.ViewModels.Entities
{
    public class BookmarkResourceUrlViewModel : ViewModelBase, IMediaEntity
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(BookmarkViewModel))]
        public long StatusId { get; set; }

        public string Url { get; set; }
    }
}
