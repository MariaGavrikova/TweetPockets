using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using LinqToTwitter;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
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
            Id = (long)model.StatusID;
            Author = model.User.Name;
            AuthorImageUrl = model.User.ProfileImageUrl;
            Text = model.Text;
            CreatedAt = model.CreatedAt;

            var urls = new List<ResourceUrlViewModel>();
            foreach (var urlEntity in model.Entities.UrlEntities)
            {
                urls.Add(new ResourceUrlViewModel()
                {
                    StatusId = Id,
                    Url = urlEntity.ExpandedUrl
                });
            }
            ResourceUrls = urls;

            var photos = new List<PhotoUrlViewModel>();
            foreach (var mediaEntity in model.Entities.MediaEntities)
            {
                photos.Add(new PhotoUrlViewModel()
                {
                    StatusId = Id,
                    Url = mediaEntity.MediaUrl
                });
            }
            PhotoUrls = photos;
        }

        [PrimaryKey]
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        [Ignore]
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

        public string Author { get; set; }

        public string AuthorImageUrl { get; set; }

        public string Text { get; set; }

        [Ignore]
        public bool CanBeReadLater
        {
            get { return ResourceUrls != null && ResourceUrls.Any(); }
        }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<PhotoUrlViewModel> PhotoUrls { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ResourceUrlViewModel> ResourceUrls { get; set; }

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
