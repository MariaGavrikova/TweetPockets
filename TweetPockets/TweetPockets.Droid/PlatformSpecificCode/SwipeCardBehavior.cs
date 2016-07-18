using System;
using System.Collections.Generic;
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
using TweetPockets.Controls;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class SwipeCardBehavior : ItemTouchHelper.Callback
    {
        private TimelineAdapter _adapter;
        private readonly TimelineListView _element;

        public SwipeCardBehavior(TimelineAdapter adapter, TimelineListView element)
        {
            this._adapter = adapter;
            _element = element;
        }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            var i = viewHolder.AdapterPosition;
            var data = _adapter.GetDataAt(i);

            int swipeFlags = data.CanBeReadLater ? ItemTouchHelper.End : 0;
            return MakeMovementFlags(0, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            var statusViewHolder = (StatusViewHolder) viewHolder;
            var data = statusViewHolder.GetData();
            _element.DismissCommand.Execute(data);
        }
    }
}