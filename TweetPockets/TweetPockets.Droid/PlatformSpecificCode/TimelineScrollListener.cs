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

                var loading = firstVisiblePosition + visibleItemCount >= totalItemCount;
                if (loading)
                {
                    _element.LoadOldCommand.Execute(null);
                }
            }
        }
    }
}