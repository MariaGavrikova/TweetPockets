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
                var navigationPage = (App.Instance.MainPage as NavigationPage);
                var mainPage = (navigationPage.CurrentPage as MainPage);
                mainPage.Detail = App.Instance.ViewManager.GetView(item);
                MostImportantItemsList.SelectedItem = null;
                mainPage.IsPresented = false;
            }
        }
    }
}
