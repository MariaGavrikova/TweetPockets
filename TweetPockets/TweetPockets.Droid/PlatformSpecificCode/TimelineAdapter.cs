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

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class TimelineAdapter : RecyclerView.Adapter
    {
        private readonly TimelineListView _element;
        private const int DefaultViewType = 0;
        private const int PhotoViewType = 1;

        private readonly BatchedObservableCollection<StatusViewModel> _items;
        private readonly Dictionary<int, int> _viewTypes;

        public TimelineAdapter(TimelineListView element)
        {
            _element = element;
            _items = (BatchedObservableCollection<StatusViewModel>) element.ItemsSource;
            _items.CollectionChanged += CollectionChangedHandler;
            _viewTypes = new Dictionary<int, int>()
            {
                { DefaultViewType, Resource.Layout.StatusCard },
                { PhotoViewType, Resource.Layout.PhotoCard }
            };
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                NotifyItemInserted(e.NewStartingIndex);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                NotifyItemRemoved(e.OldStartingIndex);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var data = _items[position];
            StatusViewHolder vh = (StatusViewHolder) holder;
            vh.Bind(data);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(_viewTypes[viewType], parent, false);
            StatusViewHolder vh = Create(itemView, viewType);
            return vh;
        }

        private StatusViewHolder Create(View itemView, int viewType)
        {
            if (viewType == PhotoViewType)
            {
                return new PhotoViewHolder(itemView, _element);
            }
            return new StatusViewHolder(itemView, _element);
        }

        public override int GetItemViewType(int position)
        {
            var data = _items[position];
            if (data is PhotoStatusViewModel)
            {
                return PhotoViewType;
            }
            return DefaultViewType;
        }

        public override int ItemCount
        {
            get { return _items.Count; }
        }

        public ItemTouchHelper ItemTouchHelper { get; set; }

        public StatusViewModel GetDataAt(int index)
        {
            return _items[index];
        }
    }
}