using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TweetPockets.Controls;

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class TimelineScrollListener : RecyclerView.OnScrollListener
    {
        private readonly LinearLayoutManager _linearLayoutManager;
        private readonly TimelineListView _element;

        public TimelineScrollListener(LinearLayoutManager linearLayoutManager, TimelineListView element)
        {
            _linearLayoutManager = linearLayoutManager;
            _element = element;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            if (dy > 0)
            {
                var visibleItemCount = _linearLayoutManager.ChildCount;
                var totalItemCount = _linearLayoutManager.ItemCount;
                var firstVisiblePosition = _linearLayoutManager.FindFirstVisibleItemPosition();

                var loadingOldRequested = firstVisiblePosition + visibleItemCount >= totalItemCount;
                if (loadingOldRequested)
                {
                    _element.LoadOldCommand?.Execute(null);
                }
            }
            else
            {
                var firstVisiblePosition = _linearLayoutManager.FindFirstCompletelyVisibleItemPosition();
                _element.CanBeUpdatedByTimer = firstVisiblePosition == 0;
            }
        }
    }
}