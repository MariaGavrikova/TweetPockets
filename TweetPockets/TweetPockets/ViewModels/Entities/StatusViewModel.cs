using System;
using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using TweetPockets.Resources;

namespace TweetPockets.ViewModels.Entities
{
    public class StatusViewModel : ViewModelBase
    {
        private readonly Status _model;
        private bool _isFavorite;
        private bool _isRetweeted;

        public StatusViewModel()
        {
            AuthorImageUrl = "https://pbs.twimg.com/profile_images/550285117454049280/u_XoHwmS_bigger.jpeg";
        }

        public StatusViewModel(Status model)
        {
            _model = model;
            Id = (long) model.StatusID;
            Author = model.User.Name;
            AuthorImageUrl = model.User.ProfileImageUrl;
            Text = model.Text;
            CreatedAt = model.CreatedAt;
            IsFavorite = model.Favorited;
            IsRetweeted = model.Retweeted;

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
                    result = String.Format(AppResources.DaysLabel, timespan.Days);
                }
                else
                {
                    if (timespan.Hours > 0)
                    {
                        result = String.Format(AppResources.HoursLabel, timespan.Hours);
                    }
                    else
                    {
                        if (timespan.Minutes > 0)
                        {
                            result = String.Format(AppResources.MinutesLabel, timespan.Minutes);
                        }
                        else
                        {
                            result = String.Format(AppResources.SecondsLabel, timespan.Seconds);
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

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                OnPropertyChanged();
            }
        }

        public bool IsRetweeted
        {
            get { return _isRetweeted; }
            set
            {
                _isRetweeted = value;
                OnPropertyChanged();
            }
        }

        public long? OldId { get; set; }

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
