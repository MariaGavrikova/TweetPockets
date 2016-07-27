using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TweetPockets.ViewModels
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
