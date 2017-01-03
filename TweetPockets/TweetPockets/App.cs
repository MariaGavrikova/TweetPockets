using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Factories;
using TweetPockets.Interfaces;
using TweetPockets.Managers;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using TweetPockets.Views;
using Xamarin.Forms;
using Xamarin.Auth;

namespace TweetPockets
{
    public class App : Application
    {
        private readonly MainViewModel _mainViewModel;

        public App()
        {
            Instance = this;

            if (Device.OS != TargetPlatform.WinPhone)
            {
                AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }

            MessagingCenter.Subscribe<App, Xamarin.Auth.Account>(this, "LoggedIn",
                async (s, user) => await OnLoggedIn(user));

            var mainPage = new Views.MainPage();
            
            _mainViewModel = new MainViewModel();
            mainPage.BindingContext = _mainViewModel;
            MainPage = mainPage;
            NavigationPage.SetHasNavigationBar(mainPage, false);
            InitPages(_mainViewModel);

            var savedAccount = AccountStore.Create().FindAccountsForService("Twitter").FirstOrDefault();
            if (savedAccount != null)
            {
                OnLoggedIn(savedAccount);
            }
        }

        private void InitPages(MainViewModel mainViewModel)
        {
            ViewManager = new ViewManager();
            ViewManager.Register<TimelineViewModel, TimelinePage>();
            ViewManager.Register<ActivityViewModel, ActivityPage>();
            ViewManager.Register<BookmarkListViewModel, BookmarkListPage>();
            ViewManager.Register<NewTweetViewModel, NewTweetPage>();
        }

        public ViewManager ViewManager { get; private set; }

        public bool IsLoggedIn { get; private set; }

        public static App Instance { get; private set; }

        private async Task OnLoggedIn(Account account)
        {
            IsLoggedIn = true;

            AccountStore.Create().Save(account, "Twitter");
            await _mainViewModel.InitAsync(account);
        }

        public void PushAsync(Page page)
        {
            var mainPage = (App.Instance.MainPage as MasterDetailPage);
            var navigationPage = (mainPage.Detail as NavigationPage);
            navigationPage.PushAsync(page);
        }

        public void PopAsync()
        {
            var mainPage = (App.Instance.MainPage as MasterDetailPage);
            var navigationPage = (mainPage.Detail as NavigationPage);
            navigationPage.PopAsync();
        }
    }
}
