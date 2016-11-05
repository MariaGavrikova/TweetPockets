using System;
using System.ComponentModel;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Droid.PlatformSpecificCode.ViewHolders
{
    class StatusViewHolder : RecyclerView.ViewHolder
    {
        private readonly TimelineListView _element;
        private StatusViewModel _data;

        public StatusViewHolder(View itemView, TimelineListView element) : base(itemView)
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
            FavoriteButton = itemView.FindViewById<ImageButton>(Resource.Id.FavoriteButton);
            FavoriteButton.Click += FavoriteClickHandler;
            BookmarkButton = itemView.FindViewById<ImageButton>(Resource.Id.BookmarkButton);
            BookmarkButton.Click += ReadLaterClickHandler;
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

        public virtual void Bind(StatusViewModel data)
        {
            if (_data != null)
            {
                _data.PropertyChanged -= PropertyChangedHandler;
            }

            _data = data;

            if (_data != null)
            {
                var worker = new BitmapWorkerTask(AuthorImage);
                worker.Execute(_data.AuthorImageUrl);

                RefreshFavoriteButton(_data);

                Text.Text = _data.Text;
                Author.Text = _data.Author;
                Timestamp.Text = _data.TimestampLabel;
                BookmarkButton.Visibility = _data.CanBeReadLater ? ViewStates.Visible : ViewStates.Invisible;
                
                _data.PropertyChanged += PropertyChangedHandler;
            }
        }

        private void RefreshFavoriteButton(StatusViewModel data)
        {
            FavoriteButton.SetImageResource(
                data.IsFavorite
                    ? Resource.Drawable.ic_favorite_green_24dp
                    : Resource.Drawable.ic_favorite_black_24dp);
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFavorite")
            {
                RefreshFavoriteButton(_data);
            }
        }
    }
}