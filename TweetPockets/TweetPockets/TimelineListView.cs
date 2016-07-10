using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TweetPockets
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
    }
}
