using System;
using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Managers;
using TweetPockets.Resources;

namespace TweetPockets.ViewModels.Entities
{
    public class StatusViewModel : ViewModelBase, ITimelineEntity
    {
        private bool _isFavorite;
        private bool _isRetweeted;
        private bool _isBookmarked;

        public StatusViewModel()
        {
        }

        public StatusViewModel(Status model, BookmarkPersistingManager persistingManager)
        {
            Id = (long) model.StatusID;
            Author = model.User.Name;
            AuthorImageUrl = model.User.ProfileImageUrl.Replace("_normal", String.Empty);
            Text = model.Text;
            CreatedAt = model.CreatedAt;
            IsFavorite = model.Favorited;
            IsRetweeted = model.Retweeted;
            IsBookmarked = persistingManager.IsStatusBookmarked(Id);

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

        [Ignore]
        IEnumerable<IMediaEntity> ITimelineEntity.PhotoUrls => PhotoUrls;

        [Ignore]
        IEnumerable<IMediaEntity> ITimelineEntity.ResourceUrls => ResourceUrls;

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

        public bool IsBookmarked
        {
            get { return _isBookmarked; }
            set
            {
                _isBookmarked = value; 
                OnPropertyChanged();
            }
        }

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
