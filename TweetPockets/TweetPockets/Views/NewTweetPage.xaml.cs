using System;
using System.Collections.Generic;
using TweetPockets.ViewModels;
using Xamarin.Forms;

namespace TweetPockets.Views
{
    public partial class NewTweetPage : ContentPage
    {
        private List<ToolbarItem> _toolbarBackup;

        public NewTweetPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var parent = Parent as NavigationPage;
            if (parent != null)
            {
                _toolbarBackup = new List<ToolbarItem>(parent.ToolbarItems);
                parent.ToolbarItems.Clear();
                parent.ToolbarItems.Add(new ToolbarItem("Send", "ic_twitter_grey600_24dp.png", OnSend));
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var parent = Parent as NavigationPage;
            if (parent != null)
            {
                parent.ToolbarItems.Clear();
                foreach (var toolbarItem in _toolbarBackup)
                {
                    parent.ToolbarItems.Add(toolbarItem);
                }
            }
        }

        private void OnSend()
        {
            var bindingContext = BindingContext as NewTweetViewModel;
            if (bindingContext != null && bindingContext.SendCommand.CanExecute(null))
            {
                bindingContext.SendCommand.Execute(null);
            }
        }
    }
}
