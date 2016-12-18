using System;
using TweetPockets.ViewModels;
using Xamarin.Forms;

namespace TweetPockets.Views
{
    public partial class SideMenuPage : ContentPage
    {
        public SideMenuPage()
        {
            InitializeComponent();

            MostImportantItemsList.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuItemViewModel;
            if (item != null)
            {
                var mainPage = (App.Instance.MainPage as MasterDetailPage);
                mainPage.Detail = new NavigationPage(App.Instance.ViewManager.GetView(item));
                MostImportantItemsList.SelectedItem = null;
                mainPage.IsPresented = false;
            }
        }
    }
}
