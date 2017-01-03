using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Utils;
using Xamarin.Auth;

namespace TweetPockets.Factories
{
    public class UserDetailsFactory
    {
        public UserDetails Create(Account account)
        {
            var userDetails = new UserDetails();
            userDetails.Token = account.Properties["oauth_token"];
            userDetails.TokenSecret = account.Properties["oauth_token_secret"];
            long id;
            long.TryParse(account.Properties["user_id"], out id);
            userDetails.TwitterId = id;
            userDetails.ScreenName = account.Properties["screen_name"];
            return userDetails;
        }
    }
}
