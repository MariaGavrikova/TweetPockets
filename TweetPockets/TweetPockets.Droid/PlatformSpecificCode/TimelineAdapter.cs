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
using TweetPockets.Utils;
using TweetPockets.ViewModels;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class TimelineAdapter : RecyclerView.Adapter
    {
        private readonly TimelineListView _element;
        private readonly RecyclerView _recycler;
        private const int DefaultViewType = 0;
        private const int PhotoViewType = 1;
        private const int FooterViewType = -1;

        private readonly BatchedObservableCollection<StatusViewModel> _items;
        private readonly Dictionary<int, int> _viewTypes;

        public TimelineAdapter(TimelineListView element, RecyclerView recycler)
        {
            _element = element;
            _recycler = recycler;
            _items = (BatchedObservableCollection<StatusViewModel>)element.ItemsSource;
            _items.CollectionChanged += CollectionChangedHandler;
            _viewTypes = new Dictionary<int, int>()
            {
                { DefaultViewType, Resource.Layout.StatusCard },
                { PhotoViewType, Resource.Layout.PhotoCard },
                { FooterViewType, Resource.Layout.RecyclerViewFooter }
            };
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);

                if (e.NewStartingIndex == 0)
                {
                    _recycler.SmoothScrollBy(0, -40);
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
                StatusViewHolder vh = (StatusViewHolder)holder;
                vh.Bind(data);
            }
            else
            {
                FooterViewHolder vh = (FooterViewHolder) holder;
                vh.Bind(_element);
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
                return new PhotoViewHolder(itemView, _element);
            }
            return new StatusViewHolder(itemView, _element);
        }

        public override int GetItemViewType(int position)
        {
            if (position == _items.Count)
            {
                return FooterViewType;
            }

            var data = _items[position];
            if (data.PhotoUrls.Any())
            {
                return PhotoViewType;
            }
            return DefaultViewType;
        }

        public override int ItemCount
        {
            get { return _items.Count + 1; }
        }

        public ItemTouchHelper ItemTouchHelper { get; set; }

        public StatusViewModel GetDataAt(int index)
        {
            return _items[index];
        }
    }
}