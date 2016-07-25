using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Java.Util;
using TweetPockets;
using TweetPockets.Controls;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using ListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(TimelineListView), typeof(TimelineListViewRenderer))]

namespace TweetPockets.Droid.Renderers
{
    public class TimelineListViewRenderer : ViewRenderer<TimelineListView, Android.Views.View>, SwipeRefreshLayout.IOnRefreshListener
    {
        private RecyclerView _recyclerView;
        private SwipeRefreshLayout _swipeRefreshLayout;
        private TimelineAdapter _adapter;

        protected override void OnElementChanged(ElementChangedEventArgs<TimelineListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var parent = new LinearLayout(Forms.Context);
                var view = (Forms.Context as Activity).LayoutInflater.Inflate(Resource.Layout.TimelineList, parent);

                _swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefreshLayout);
                _swipeRefreshLayout.SetOnRefreshListener(this);
                _swipeRefreshLayout.Enabled = Element.IsPullToRefreshEnabled;
                _swipeRefreshLayout.Refreshing = Element.IsRefreshing;

                _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.RecyclerView);
                _adapter = new TimelineAdapter(Element, _recyclerView);
                _recyclerView.SetAdapter(_adapter);
                var linearLayoutManager = new LinearLayoutManager(Forms.Context);
                _recyclerView.SetLayoutManager(linearLayoutManager);
                //ItemTouchHelper itemTouchHelper = new ItemTouchHelper(new SwipeCardBehavior(_adapter, Element));
                //itemTouchHelper.AttachToRecyclerView(_recyclerView);
                //_adapter.ItemTouchHelper = itemTouchHelper;

                SetNativeControl(view);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == TimelineListView.IsRefreshingProperty.PropertyName)
            {
                _swipeRefreshLayout.Refreshing = Element.IsRefreshing;
            }
        }

        public void OnRefresh()
        {
            Element.RefreshCommand.Execute(null);
        }
    }
}