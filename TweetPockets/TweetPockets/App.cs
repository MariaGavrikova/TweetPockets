using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Auth;

namespace TweetPockets
{
    public class App : Application
    {
        private UserDetails _user;
        private TimelinePage _timelineView;

        public App()
        {
            Instance = this;

            MessagingCenter.Subscribe<App, Xamarin.Auth.Account>(this, "LoggedIn",
                async (s, user) => await OnLoggedIn(user));

            var mainPage = new MainPage();
            _timelineView = mainPage.Timeline;
            MainPage = mainPage;

            var savedAccount = AccountStore.Create().FindAccountsForService("Twitter").FirstOrDefault();
            if (savedAccount != null)
            {
                OnLoggedIn(savedAccount);
            }
        }

        public bool IsLoggedIn { get; private set; }

        public static App Instance { get; private set; }

        private async Task OnLoggedIn(Account account)
        {
            IsLoggedIn = true;

            var userDetails = new UserDetails();
            userDetails.Token = account.Properties["oauth_token"];
            userDetails.TokenSecret = account.Properties["oauth_token_secret"];
            userDetails.TwitterId = account.Properties["user_id"];
            userDetails.ScreenName = account.Properties["screen_name"];

            _user = userDetails;

            var timeline = new TimelineViewModel();
            _timelineView.BindingContext = timeline;

            await timeline.InitAsync(_user);

            AccountStore.Create().Save(account, "Twitter");
        }
    }
}
