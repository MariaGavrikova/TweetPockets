using System;
using System.Linq;
using System.Reactive;
using LinqToTwitter;
using TweetPockets.Utils;

namespace TweetPockets.ViewModels
{
    public class StatusViewModel : ViewModelBase
    {
        public StatusViewModel()
        {
            AuthorImageUrl = "https://pbs.twimg.com/profile_images/550285117454049280/u_XoHwmS_bigger.jpeg";
        }

        public StatusViewModel(Status model)
        {
            Id = model.StatusID;
            Author = model.User.Name;
            AuthorImageUrl = model.User.ProfileImageUrl;
            Text = model.Text;
            CreatedAt = model.CreatedAt;

            CanBeReadLater = model.Entities.UrlEntities.Any();
        }

        public DateTime CreatedAt { get; }

        public string TimestampLabel
        {
            get
            {
                var timespan = DateTime.UtcNow - CreatedAt;
                string result;
                if (timespan.Days > 0)
                {
                    result = $"{timespan.Days}d";
                }
                else
                {
                    if (timespan.Hours > 0)
                    {
                        result = $"{timespan.Hours}h";
                    }
                    else
                    {
                        if (timespan.Minutes > 0)
                        {
                            result = $"{timespan.Minutes}m";
                        }
                        else
                        {
                            result = $"{timespan.Seconds}s";
                        }
                    }
                }

                return result;
            }
        }

        public ulong Id { get; }

        public string Author { get; }

        public string AuthorImageUrl { get; }

        public string Text { get; }

        public bool CanBeReadLater { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null)
            {
                isEqual = GetHashCode() == obj.GetHashCode();
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
