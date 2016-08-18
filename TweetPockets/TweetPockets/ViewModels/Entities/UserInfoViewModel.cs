using SQLite.Net.Attributes;

namespace TweetPockets.ViewModels.Entities
{
    public class UserInfoViewModel : ViewModelBase
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string AvatarUrl { get; set; }

        public string ScreenName { get; set; }

        public string Name { get; set; }

        [Ignore]
        public string SplashUrl { get; set; } = "https://unsplash.it/300/200/?random";
    }
}
