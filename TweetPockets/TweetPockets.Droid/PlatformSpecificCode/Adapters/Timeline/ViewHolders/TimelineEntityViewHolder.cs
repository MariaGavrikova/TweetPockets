using System;
using System.ComponentModel;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.Interfaces.Entities;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Droid.PlatformSpecificCode.Adapters.Timeline.ViewHolders
{
    class TimelineEntityViewHolder : RecyclerView.ViewHolder
    {
        private readonly TimelineListView _element;
        private ITimelineEntity _data;

        public TimelineEntityViewHolder(View itemView, TimelineListView element) : base(itemView)
        {
            _element = element;
            Card = itemView.FindViewById<CardView>(Resource.Id.Card);
            AuthorImage = itemView.FindViewById<ImageView>(Resource.Id.AuthorImage);
            Author = itemView.FindViewById<TextView>(Resource.Id.Author);
            Text = itemView.FindViewById<TextView>(Resource.Id.Text);
            Timestamp = itemView.FindViewById<TextView>(Resource.Id.Timestamp);
            ReplyButton = itemView.FindViewById<ImageButton>(Resource.Id.ReplyButton);
            ReplyButton.Click += ReplyClickHandler;
            RetweetButton = itemView.FindViewById<ImageButton>(Resource.Id.RetweetButton);
            RetweetButton.Click += RetweetClickHandler;
            FavoriteButton = itemView.FindViewById<ImageButton>(Resource.Id.FavoriteButton);
            FavoriteButton.Click += FavoriteClickHandler;
            BookmarkButton = itemView.FindViewById<ImageButton>(Resource.Id.BookmarkButton);
            BookmarkButton.Click += ReadLaterClickHandler;
        }

        private void RetweetClickHandler(object sender, EventArgs e)
        {
            _element.RetweetCommand.Execute(_data);
        }

        private void FavoriteClickHandler(object sender, EventArgs e)
        {
            _element.FavoriteCommand.Execute(_data);
        }

        private void ReadLaterClickHandler(object sender, EventArgs e)
        {
            _element.DismissCommand.Execute(_data);
        }

        private void ReplyClickHandler(object sender, EventArgs e)
        {
            _element.ReplyCommand.Execute(_data);
        }

        public ImageButton BookmarkButton { get; set; }
        public ImageButton FavoriteButton { get; set; }
        public ImageButton RetweetButton { get; set; }
        public ImageButton ReplyButton { get; set; }
        public CardView Card { get; private set; }
        public ImageView AuthorImage { get; private set; }
        public TextView Author { get; private set; }
        public TextView Text { get; private set; }
        public TextView Timestamp { get; private set; }

        public virtual void Bind(ITimelineEntity data)
        {
            if (_data != null)
            {
                _data.PropertyChanged -= PropertyChangedHandler;
            }

            _data = data;

            if (_data != null)
            {
                AuthorImage.SetImageBitmap(null);
                var worker = new BitmapWorkerTask(AuthorImage);
                worker.Execute(_data.AuthorImageUrl);

                RefreshReplyButton(_data);

                RefreshFavoriteButton(_data);

                RefreshRetweetButton(_data);

                RefreshBookmarkButton(_data);

                Text.Text = _data.Text;
                Author.Text = _data.Author;
                Timestamp.Text = _data.TimestampLabel;
                
                _data.PropertyChanged += PropertyChangedHandler;
            }
        }

        private void RefreshReplyButton(ITimelineEntity data)
        {
            if (data is BookmarkViewModel)
            {
                ReplyButton.Visibility = ViewStates.Invisible;
            }
            else
            {
                ReplyButton.Visibility = ViewStates.Visible;
            }
        }

        private void RefreshFavoriteButton(ITimelineEntity data)
        {
            if (data is BookmarkViewModel)
            {
                FavoriteButton.Visibility = ViewStates.Invisible;
            }
            else
            {
                FavoriteButton.Visibility = ViewStates.Visible;
                FavoriteButton.SetImageResource(
                    data.IsFavorite
                        ? Resource.Drawable.ic_favorite_green_24dp
                        : Resource.Drawable.ic_favorite_black_24dp);
            }
        }

        private void RefreshRetweetButton(ITimelineEntity data)
        {
            if (data is BookmarkViewModel)
            {
                RetweetButton.Visibility = ViewStates.Invisible;
            }
            else
            {
                RetweetButton.Visibility = ViewStates.Visible;
                RetweetButton.SetImageResource(
                    data.IsRetweeted
                        ? Resource.Drawable.ic_repeat_green_24dp
                        : Resource.Drawable.ic_repeat_black_24dp);
            }
        }

        private void RefreshBookmarkButton(ITimelineEntity data)
        {
            if (data is BookmarkViewModel)
            {
                BookmarkButton.SetImageResource(Resource.Drawable.ic_trash_black_24dp);
            }
            else
            { 
                BookmarkButton.SetImageResource(
                    data.IsBookmarked
                        ? Resource.Drawable.ic_book_green_24dp
                        : Resource.Drawable.ic_book_black_24dp);
                BookmarkButton.Visibility = _data.CanBeReadLater ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFavorite")
            {
                RefreshFavoriteButton(_data);
            }
            if (e.PropertyName == "IsRetweeted")
            {
                RefreshRetweetButton(_data);
            }
            if (e.PropertyName == "IsBookmarked")
            {
                RefreshBookmarkButton(_data);
            }
        }
    }
}