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
using TweetPockets.Droid.PlatformSpecificCode.Adapters;
using TweetPockets.Droid.PlatformSpecificCode.Adapters.Activity;
using TweetPockets.Droid.PlatformSpecificCode.Adapters.Timeline;
using TweetPockets.Droid.Renderers;
using TweetPockets.Interfaces;
using TweetPockets.Interfaces.Entities;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using ListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(TimelineListView), typeof(TimelineListViewRenderer))]

namespace TweetPockets.Droid.Renderers
{
    public class TimelineListViewRenderer : ViewRenderer<TimelineListView, Android.Views.View>,
        SwipeRefreshLayout.IOnRefreshListener
    {
        private RecyclerView _recyclerView;
        private SwipeRefreshLayout _swipeRefreshLayout;
        private RecyclerView.Adapter _adapter;

        private IDictionary<Type, Func<TimelineListView,RecyclerView, LinearLayoutManager, RecyclerView.Adapter>> _adapterFactories =
            new Dictionary<Type, Func<TimelineListView, RecyclerView, LinearLayoutManager, RecyclerView.Adapter>>()
            {
                { typeof(ITimelineEntity), (e,r,l) => new TimelineAdapter(e, r, l) },
                { typeof(EventViewModel), (e,r,l) => new ActivityAdapter(e, r, l) },
            };

        protected override void OnElementChanged(ElementChangedEventArgs<TimelineListView> e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement;
            if (element != null)
            {
                var parent = new LinearLayout(Forms.Context);
                var view = (Forms.Context as Activity).LayoutInflater.Inflate(Resource.Layout.TimelineList, parent);

                _swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefreshLayout);
                _swipeRefreshLayout.SetOnRefreshListener(this);
                _swipeRefreshLayout.Enabled = element.IsPullToRefreshEnabled;
                _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.RecyclerView);
               
                var linearLayoutManager = new LinearLayoutManager(Forms.Context);
                _recyclerView.SetLayoutManager(linearLayoutManager);
                _adapter = CreateAdapter(element, _recyclerView, linearLayoutManager);
                _recyclerView.SetAdapter(_adapter);
                _recyclerView.AddOnScrollListener(new TimelineScrollListener(linearLayoutManager, element));

                SetNativeControl(view);
            }
        }

        private RecyclerView.Adapter CreateAdapter(
            TimelineListView element,
            RecyclerView recyclerView, 
            LinearLayoutManager linearLayoutManager)
        {
            var itemsSource = element.ItemsSource as IGenericCollection;
            var itemType = itemsSource.ItemType;
            var factory = _adapterFactories[itemType];
            return factory.Invoke(element, recyclerView, linearLayoutManager);
        }

        private void ScrollToTopHandler(object sender, EventArgs e)
        {
            _recyclerView.SmoothScrollToPosition(0);
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