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
            ReadLaterButton = itemView.FindViewById<ImageButton>(Resource.Id.ReadLaterButton);
            ReadLaterButton.Click += ReadLaterClickHandler;
        }

        private void ReadLaterClickHandler(object sender, EventArgs e)
        {
            var i = AdapterPosition;
            _element.DismissCommand.Execute(i);
        }

        private void ReplyClickHandler(object sender, EventArgs e)
        {
            
        }

        public ImageButton ReadLaterButton { get; set; }
        public ImageButton FavoriteButton { get; set; }
        public ImageButton RetweetButton { get; set; }
        public ImageButton ReplyButton { get; set; }
        public CardView Card { get; private set; }
        public ImageView AuthorImage { get; private set; }
        public TextView Author { get; private set; }
        public TextView Text { get; private set; }

        public virtual void Bind(StatusViewModel data)
        {
            AuthorImage.SetImageBitmap(BitmapUtils.GetImageBitmapFromUrl(data.AuthorImageUrl, 50, 50));
            Text.Text = data.Text;
            Author.Text = data.Author;
            ReadLaterButton.Visibility = data.CanBeReadLater ? ViewStates.Visible : ViewStates.Gone;

            var layoutParams = (RecyclerView.LayoutParams)Card.LayoutParameters;
            if (this.AdapterPosition == 0)
            {
                layoutParams.TopMargin = layoutParams.BottomMargin;
            }
            else
            {
                layoutParams.TopMargin = 0;
            }
            Card.LayoutParameters = layoutParams;
        }
    }
}