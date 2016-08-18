using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TweetPockets.ViewModels.Entities
{
    public class PhotoUrlViewModel : ViewModelBase
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(StatusViewModel))]
        public long StatusId { get; set; }

        public string Url { get; set; }
    }
}
