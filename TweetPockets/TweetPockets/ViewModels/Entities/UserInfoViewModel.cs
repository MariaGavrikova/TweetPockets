using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace TweetPockets.ViewModels.Entities
{
    public class UserInfoViewModel : ViewModelBase
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string AvatarUrl { get; set; }

        public string ScreenName { get; set; }

        public string Name { get; set; }

        public string BannerUrl { get; set; }

        public string BackgroundColorString { get; set; }

        [Ignore]
        public Color BackgroundColor
        {
            get { return Color.FromHex(BackgroundColorString); }
        }
        
    }
}
