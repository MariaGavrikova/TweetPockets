using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode.Cache;
using TweetPockets.Interfaces.Entities;

namespace TweetPockets.Droid.PlatformSpecificCode.Adapters.Timeline.ViewHolders
{
    class SinglePhotoViewHolder : TimelineEntityViewHolder
    {
        private CancellationTokenSource _photoLoadingCancellation;
        private Task _photoLoadingTask;

        public SinglePhotoViewHolder(View itemView, TimelineListView element) : 
            base(itemView, element)
        {
            Photo = itemView.FindViewById<ImageView>(Resource.Id.Photo);
        }

        public ImageView Photo { get; private set; }

        public override void Bind(ITimelineEntity data)
        {
            _photoLoadingCancellation?.Cancel();
            _photoLoadingTask?.Dispose();

            base.Bind(data);

            Photo.SetImageBitmap(null);
            _photoLoadingCancellation = new CancellationTokenSource();
            _photoLoadingTask = Task
                .Run(() => BitmapCache.Instance.Get(data.PhotoUrls.FirstOrDefault().Url), _photoLoadingCancellation.Token)
                .ContinueWith(t => LoadImage(t, Photo),
                    _photoLoadingCancellation.Token,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}