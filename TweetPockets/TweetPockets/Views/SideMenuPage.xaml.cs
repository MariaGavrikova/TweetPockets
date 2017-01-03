using System;
using TweetPockets.ViewModels;
using Xamarin.Forms;

namespace TweetPockets.Views
{
    public partial class SideMenuPage : ContentPage
    {
        private MainViewModel _data;

        public SideMenuPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _data = BindingContext as MainViewModel;
        }

        private async void ItemTapped(object sender, EventArgs e)
        {
            var view = sender as View;
            var item = view?.BindingContext as MenuItemViewModel;
            if (item != null)
            {
                var mainPage = (App.Instance.MainPage as MasterDetailPage);
                mainPage.Detail = new NavigationPage(App.Instance.ViewManager.GetView(item));
                mainPage.IsPresented = false;

                _data.SelectedItem = item;
                using (item.ActivityLoading())
                {
                    await item.Reload();
                }
            }
        }
    }
}
