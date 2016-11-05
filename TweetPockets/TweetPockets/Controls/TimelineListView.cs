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
    }
}
