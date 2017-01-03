using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TweetPockets.Interfaces.Entities
{
    public interface ITimelineEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
        string TimestampLabel { get; }
        string Author { get; set; }
        string AuthorImageUrl { get; set; }
        string Text { get; set; }
        bool CanBeReadLater { get; }
        IEnumerable<IMediaEntity> PhotoUrls { get; }
        IEnumerable<IMediaEntity> ResourceUrls { get; }
        bool IsFavorite { get; set; }
        bool IsRetweeted { get; set; }
        long? OldId { get; set; }
        bool IsBookmarked { get; set; }
    }
}