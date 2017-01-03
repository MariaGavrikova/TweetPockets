using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Resources;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Droid.PlatformSpecificCode.Adapters.Activity.ViewHolders
{
    class EventViewHolder : RecyclerView.ViewHolder
    {
        private IDictionary<UserStreamEventType, int> _icons = new Dictionary<UserStreamEventType, int>()
        {
            { UserStreamEventType.Favorite, Resource.Drawable.ic_favorite_red_24dp },
            { UserStreamEventType.Retweet, Resource.Drawable.ic_repeat_blue_24dp },
            { UserStreamEventType.Quoted, Resource.Drawable.ic_quote_black_24dp }
        };

        private IDictionary<UserStreamEventType, string> _texts = new Dictionary<UserStreamEventType, string>()
        {
            { UserStreamEventType.Favorite, AppResources.LabelFavorited },
            { UserStreamEventType.Retweet, AppResources.LabelRetweeted },
            { UserStreamEventType.Quoted, AppResources.LabelQuoted }
        };

        public EventViewHolder(View itemView) : base(itemView)
        {
            EventTypeImage = itemView.FindViewById<ImageView>(Resource.Id.EventType);
            Author = itemView.FindViewById<TextView>(Resource.Id.Author);
            EventTypeText = itemView.FindViewById<TextView>(Resource.Id.EventTypeText);
            TimestampText = itemView.FindViewById<TextView>(Resource.Id.Timestamp);
            StatusText = itemView.FindViewById<TextView>(Resource.Id.Text);
        }

        public TextView StatusText { get; set; }

        public TextView TimestampText { get; set; }

        public TextView EventTypeText { get; set; }

        public TextView Author { get; set; }

        public ImageView EventTypeImage { get; set; }

        public virtual void Bind(EventViewModel data)
        {
            int image;
            _icons.TryGetValue(data.EventType, out image);
            EventTypeImage.SetImageResource(image);

            Author.Text = data.InitiatorName;
            string text;
            _texts.TryGetValue(data.EventType, out text);
            EventTypeText.Text = text;

            TimestampText.Text = data.TimestampLabel;

            StatusText.Text = data.Text;
        }
    }
}