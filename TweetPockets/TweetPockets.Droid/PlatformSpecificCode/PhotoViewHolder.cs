using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.ViewModels;

namespace TweetPockets.Droid.PlatformSpecificCode
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

            var photoData = (PhotoStatusViewModel) data;

            Photo.SetImageBitmap(null);

            var worker = new BitmapWorkerTask(Photo);
            worker.Execute(photoData.PhotoUrls.FirstOrDefault());
        }
    }
}