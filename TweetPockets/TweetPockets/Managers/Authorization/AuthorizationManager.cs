using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using TweetPockets.Factories;
using TweetPockets.Utils;
using Account = Xamarin.Auth.Account;

namespace TweetPockets.Managers.Authorization
{
    public class AuthorizationManager
    {
        private TwitterContext _context;
        private static readonly AuthorizationManager _instance = new AuthorizationManager();

        public static AuthorizationManager Instance => _instance;

        public UserDetails CurrentUserDetails { get; private set; }

        public async Task<TwitterContext> GetContext(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var userDetails = new UserDetailsFactory().Create(account);

            if (_context == null || CurrentUserDetails?.TwitterId != userDetails.TwitterId)
            {
                CurrentUserDetails = userDetails;
                var auth = new SingleUserAuthorizer()
                {
                    CredentialStore = new InMemoryCredentialStore()
                    {
                        ConsumerKey = Keys.TwitterConsumerKey,
                        ConsumerSecret = Keys.TwitterConsumerSecret,
                        OAuthToken = CurrentUserDetails.Token,
                        OAuthTokenSecret = CurrentUserDetails.TokenSecret,
                    },
                };
                await auth.AuthorizeAsync();

                _context = new TwitterContext(auth);
            }

            return _context;
        }
    }
}
