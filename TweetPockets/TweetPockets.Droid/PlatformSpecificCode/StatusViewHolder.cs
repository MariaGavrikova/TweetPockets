using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.Droid.Utilities;
using TweetPockets.ViewModels;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class StatusViewHolder : RecyclerView.ViewHolder
    {
        private readonly TimelineListView _element;

        public StatusViewHolder(View itemView, TimelineListView element) : base(itemView)
        {
            _element = element;
            Card = itemView.FindViewById<CardView>(Resource.Id.Card);
            AuthorImage = itemView.FindViewById<ImageView>(Resource.Id.AuthorImage);
            Author = itemView.FindViewById<TextView>(Resource.Id.Author);
            Text = itemView.FindViewById<TextView>(Resource.Id.Text);
            ReplyButton = itemView.FindViewById<ImageButton>(Resource.Id.ReplyButton);
            ReplyButton.Click += ReplyClickHandler;
            RetweetButton = itemView.FindViewById<ImageButton>(Resource.Id.RetweetButton);
            FavoriteButton = itemView.FindViewById<ImageButton>(Resource.Id.FavoriteButton);
            BookmardButton = itemView.FindViewById<ImageButton>(Resource.Id.BookmardButton);
            BookmardButton.Click += ReadLaterClickHandler;
        }

        private void ReadLaterClickHandler(object sender, EventArgs e)
        {
            var data = GetData();
            _element.DismissCommand.Execute(data);
        }

        private void ReplyClickHandler(object sender, EventArgs e)
        {
            
        }

        public ImageButton BookmardButton { get; set; }
        public ImageButton FavoriteButton { get; set; }
        public ImageButton RetweetButton { get; set; }
        public ImageButton ReplyButton { get; set; }
        public CardView Card { get; private set; }
        public ImageView AuthorImage { get; private set; }
        public TextView Author { get; private set; }
        public TextView Text { get; private set; }

        public StatusViewModel GetData()
        {
            return (Card.Tag as JavaObjectWrapper<StatusViewModel>).Value;
        }

        public virtual void Bind(StatusViewModel data)
        {
            var worker = new BitmapWorkerTask(AuthorImage);
            worker.Execute(data.AuthorImageUrl);

            Text.Text = data.Text;
            Author.Text = data.Author;
            BookmardButton.Visibility = data.CanBeReadLater ? ViewStates.Visible : ViewStates.Gone;

            Card.Tag = new JavaObjectWrapper<StatusViewModel>(data);
        }
    }
}