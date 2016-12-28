using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Net;
using Java.Util.Concurrent;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode.ViewHolders;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;
using Math = System.Math;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class TimelineAdapter : RecyclerView.Adapter
    {
        private readonly TimelineListView _element;
        private readonly RecyclerView _recycler;
        private readonly LinearLayoutManager _layoutManager;
        private const int StatusViewType = 0;
        private const int PhotoViewType = 1;
        private const int FooterViewType = -1;

        private readonly BatchedObservableCollection<ITimelineEntity> _items;
        private readonly Dictionary<int, int> _viewTypes;
        private readonly Handler _mainHandler;
        private IScheduledFuture _updateFuture;
        private const int ScrollingOffset = 40;

        public TimelineAdapter(TimelineListView element, RecyclerView recycler, LinearLayoutManager layoutManager)
        {
            _element = element;
            _recycler = recycler;
            _layoutManager = layoutManager;
            _items = (BatchedObservableCollection<ITimelineEntity>)element.ItemsSource;
            _items.CollectionChanged += CollectionChangedHandler;
            _viewTypes = new Dictionary<int, int>()
            {
                { StatusViewType, Resource.Layout.StatusCard },
                { PhotoViewType, Resource.Layout.PhotoCard },
                { FooterViewType, Resource.Layout.RecyclerViewFooter }
            };
            _mainHandler = new Handler(Looper.MainLooper);
            _updateFuture = Executors.NewSingleThreadScheduledExecutor()
                .ScheduleAtFixedRate(new Runnable(OnTick), 0, 30000, TimeUnit.Milliseconds);
        }

        private void OnTick()
        {
            int firstVisible = _layoutManager.FindFirstVisibleItemPosition();
            int lastVisible = _layoutManager.FindLastVisibleItemPosition();

            _mainHandler.Post(new Runnable(() => UpdateTimeStamp(firstVisible, lastVisible)));
        }

        private void UpdateTimeStamp(int fromPosition, int toPosition)
        {
            if (fromPosition > -1 && toPosition > -1)
            {
                for (int i = fromPosition; i <= toPosition; i++)
                {
                    if (i < _items.Count)
                    {
                        var item = _layoutManager.FindViewByPosition(i);
                        var timestamp = item.FindViewById<TextView>(Resource.Id.Timestamp);
                        timestamp.Text = _items[i].TimestampLabel;
                    }
                }
            }
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == 0)
                {
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);

                    if (_layoutManager.FindFirstCompletelyVisibleItemPosition() == 0)
                    {
                        _recycler.SmoothScrollBy(0, -ScrollingOffset);
                    }
                }
                else
                {
                    NotifyItemRemoved(e.NewStartingIndex);
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                NotifyItemRemoved(e.OldStartingIndex);
            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                NotifyItemRangeChanged(0, Math.Max(e.OldItems.Count, e.NewItems.Count));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position != _items.Count)
            {
                var data = _items[position];
                TimelineEntityViewHolder vh = (TimelineEntityViewHolder)holder;
                vh.Bind(data);
            }
            else
            {
                FooterViewHolder vh = (FooterViewHolder)holder;
                vh.Bind(_items);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(_viewTypes[viewType], parent, false);
            return Create(itemView, viewType);
        }

        private RecyclerView.ViewHolder Create(View itemView, int viewType)
        {
            if (viewType == FooterViewType)
            {
                return new FooterViewHolder(itemView);
            }

            if (viewType == PhotoViewType)
            {
                return new SinglePhotoViewHolder(itemView, _element);
            }
            return new TimelineEntityViewHolder(itemView, _element);
        }

        public override int GetItemViewType(int position)
        {
            if (position == _items.Count)
            {
                return FooterViewType;
            }

            var data = _items[position];
            if (data.PhotoUrls != null && data.PhotoUrls.Any())
            {
                return PhotoViewType;
            }
            return StatusViewType;
        }

        public override int ItemCount
        {
            get { return _items.Count + 1; }
        }

        public ItemTouchHelper ItemTouchHelper { get; set; }

        public ITimelineEntity GetDataAt(int index)
        {
            return _items[index];
        }
    }
}