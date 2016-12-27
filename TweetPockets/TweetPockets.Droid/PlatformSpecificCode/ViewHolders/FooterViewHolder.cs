using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;

namespace TweetPockets.Droid.PlatformSpecificCode.ViewHolders
{
    class FooterViewHolder : RecyclerView.ViewHolder
    {
        public FooterViewHolder(View itemView) : base(itemView)
        {
            CircularProgress = itemView.FindViewById<ProgressBar>(Resource.Id.CircularProgress);
        }

        public ProgressBar CircularProgress { get; set; }

        public void Bind(BatchedObservableCollection<ITimelineEntity> items)
        {
            CircularProgress.Visibility = items.HasMoreItems ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}