using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;

namespace TweetPockets.Droid.PlatformSpecificCode.ViewHolders
{
    class FooterViewHolder : RecyclerView.ViewHolder
    {
        public FooterViewHolder(View itemView) : base(itemView)
        {
            //CircularProgress = itemView.FindViewById<ProgressBar>(Resource.Id.CircularProgress);
        }

        public ProgressBar CircularProgress { get; set; }

        public void Bind(TimelineListView element)
        {
            //CircularProgress.Visibility = element.IsLoadingMore ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}