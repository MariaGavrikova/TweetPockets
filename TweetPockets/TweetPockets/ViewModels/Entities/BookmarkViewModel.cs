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
    public class BookmarkViewModel : ViewModelBase, ITimelineEntity
    {
        private bool _isFavorite;
        private bool _isRetweeted;

        public BookmarkViewModel()
        {
        }

        public BookmarkViewModel(StatusViewModel status)
        {
            Id = (long) status.Id;
            Author = status.Author;
            AuthorImageUrl = status.AuthorImageUrl;
            Text = status.Text;
            CreatedAt = status.CreatedAt;
            IsFavorite = status.IsFavorite;
            IsRetweeted = status.IsRetweeted;
            IsBookmarked = true;

            var urls = new List<BookmarkResourceUrlViewModel>();
            foreach (var urlEntity in status.ResourceUrls)
            {
                urls.Add(new BookmarkResourceUrlViewModel()
                {
                    StatusId = Id,
                    Url = urlEntity.Url
                });
            }
            ResourceUrls = urls;

            var photos = new List<BookmarkPhotoUrlViewModel>();
            foreach (var mediaEntity in status.PhotoUrls)
            {
                photos.Add(new BookmarkPhotoUrlViewModel()
                {
                    StatusId = Id,
                    Url = mediaEntity.Url
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
        public List<BookmarkPhotoUrlViewModel> PhotoUrls { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<BookmarkResourceUrlViewModel> ResourceUrls { get; set; }

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

        public bool IsBookmarked { get; set; }

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
