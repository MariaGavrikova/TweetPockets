using System.Windows.Input;
using Xamarin.Forms;

namespace TweetPockets.Controls
{
    public class TimelineListView : ListView
    {
        public static readonly BindableProperty LoadOldCommandProperty =
            BindableProperty.Create<TimelineListView, ICommand>(p => p.LoadOldCommand, null);

        public ICommand LoadOldCommand
        {
            get
            {
                return (ICommand)GetValue(LoadOldCommandProperty);
            }
            set
            {
                SetValue(LoadOldCommandProperty, value);
            }
        }

        public static readonly BindableProperty DismissCommandProperty =
            BindableProperty.Create<TimelineListView, ICommand>(p => p.DismissCommand, null);

        public ICommand DismissCommand
        {
            get
            {
                return (ICommand)GetValue(DismissCommandProperty);
            }
            set
            {
                SetValue(DismissCommandProperty, value);
            }
        }

        public static readonly BindableProperty ReplyCommandProperty =
            BindableProperty.Create<TimelineListView, ICommand>(p => p.ReplyCommand, null);

        public ICommand ReplyCommand
        {
            get
            {
                return (ICommand)GetValue(ReplyCommandProperty);
            }
            set
            {
                SetValue(ReplyCommandProperty, value);
            }
        }

        public static readonly BindableProperty FavoriteCommandProperty =
            BindableProperty.Create<TimelineListView, ICommand>(p => p.FavoriteCommand, null);

        public ICommand FavoriteCommand
        {
            get
            {
                return (ICommand)GetValue(FavoriteCommandProperty);
            }
            set
            {
                SetValue(FavoriteCommandProperty, value);
            }
        }

        public static readonly BindableProperty RetweetCommandProperty =
            BindableProperty.Create<TimelineListView, ICommand>(p => p.RetweetCommand, null);

        public ICommand RetweetCommand
        {
            get
            {
                return (ICommand)GetValue(RetweetCommandProperty);
            }
            set
            {
                SetValue(RetweetCommandProperty, value);
            }
        }

        public static readonly BindableProperty IsLoadingMoreProperty =
            BindableProperty.Create<TimelineListView, bool>(p => p.IsLoadingMore, false);

        public bool IsLoadingMore
        {
            get
            {
                return (bool)GetValue(IsLoadingMoreProperty);
            }
            set
            {
                SetValue(IsLoadingMoreProperty, value);
            }
        }

        public static readonly BindableProperty CanBeUpdatedByTimerProperty =
            BindableProperty.Create<TimelineListView, bool>(p => p.CanBeUpdatedByTimer, false);

        public bool CanBeUpdatedByTimer
        {
            get
            {
                return (bool)GetValue(CanBeUpdatedByTimerProperty);
            }
            set
            {
                SetValue(CanBeUpdatedByTimerProperty, value);
            }
        }
    }
}
