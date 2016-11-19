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
            this.InputField.Valid += (s, e) => ShowSendButton();
            this.InputField.Invalid += (s, e) => HideSendButton();
        }

        private void ShowSendButton()
        {
            var parent = Parent as NavigationPage;
            if (parent != null && parent.ToolbarItems.Count < 1)
            {
                parent.ToolbarItems.Add(new ToolbarItem("Send", "ic_twitter_grey600_24dp.png", OnSend));
            }
        }

        private void HideSendButton()
        {
            var parent = Parent as NavigationPage;
            parent?.ToolbarItems.Clear();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var parent = Parent as NavigationPage;
            if (parent != null)
            {
                _toolbarBackup = new List<ToolbarItem>(parent.ToolbarItems);
                parent.ToolbarItems.Clear();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var parent = Parent as NavigationPage;
            if (parent != null)
            {
                HideSendButton();
                foreach (var toolbarItem in _toolbarBackup)
                {
                    parent.ToolbarItems.Add(toolbarItem);
                }
            }
        }

        private void OnSend()
        {
            var bindingContext = BindingContext as NewTweetViewModel;
            bindingContext?.SendCommand.Execute(null);
        }
    }
}
