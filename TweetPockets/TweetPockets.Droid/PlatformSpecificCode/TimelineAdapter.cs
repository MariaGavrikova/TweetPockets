using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Java.Net;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode.ViewHolders;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

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
                FooterViewHolder vh = (FooterViewHolder) holder;
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