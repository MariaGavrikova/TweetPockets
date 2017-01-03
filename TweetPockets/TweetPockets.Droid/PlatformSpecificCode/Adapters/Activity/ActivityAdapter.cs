using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Java.Util.Concurrent;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode.Adapters.Activity.ViewHolders;
using TweetPockets.Droid.PlatformSpecificCode.Adapters.Timeline.ViewHolders;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Droid.PlatformSpecificCode.Adapters.Activity
{
    class ActivityAdapter : RecyclerView.Adapter
    {
        private readonly TimelineListView _element;
        private const int EventItemType = 0;

        private readonly BatchedObservableCollection<EventViewModel> _items;
        private readonly Dictionary<int, int> _viewTypes;

        public ActivityAdapter(TimelineListView element, RecyclerView recycler, LinearLayoutManager layoutManager)
        {
            _element = element;
            _items = (BatchedObservableCollection<EventViewModel>)element.ItemsSource;
            _items.CollectionChanged += CollectionChangedHandler;
            _viewTypes = new Dictionary<int, int>()
            {
                { EventItemType, Resource.Layout.EventItem },
            };
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == 0)
                {
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
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
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
               NotifyDataSetChanged();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var data = _items[position];
            EventViewHolder vh = (EventViewHolder)holder;
            vh.Bind(data);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(_viewTypes[viewType], parent, false);
            return Create(itemView, viewType);
        }

        private RecyclerView.ViewHolder Create(View itemView, int viewType)
        {
            //if (viewType == EventItemType)
            //{
            return new EventViewHolder(itemView);
            //}
        }

        public override int GetItemViewType(int position)
        {
            return EventItemType;
        }

        public override int ItemCount
        {
            get { return _items.Count; }
        }

        public EventViewModel GetDataAt(int index)
        {
            return _items[index];
        }
    }
}