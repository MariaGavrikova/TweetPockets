using System.Linq;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Droid.PlatformSpecificCode.ViewHolders
{
    class PhotoViewHolder : StatusViewHolder
    {
        public PhotoViewHolder(View itemView, TimelineListView element) : 
            base(itemView, element)
        {
            Photo = itemView.FindViewById<ImageView>(Resource.Id.Photo);
        }

        public ImageView Photo { get; private set; }

        public override void Bind(StatusViewModel data)
        {
            base.Bind(data);

            Photo.SetImageBitmap(null);

            var worker = new BitmapWorkerTask(Photo);
            worker.Execute(data.PhotoUrls.FirstOrDefault().Url);
        }
    }
}