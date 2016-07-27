using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using TweetPockets.Utils;
using TweetPockets.ViewModels;

namespace TweetPockets.Managers
{
    public class StatusLoadingManager
    {
        private int ChunkSize = 20;
        private readonly StatusPersistingManager _persistingManager;
        private TwitterContext _ctx;

        public StatusLoadingManager(StatusPersistingManager persistingManager)
        {
            _persistingManager = persistingManager;
        }

        public async Task Init(UserDetails userDetails)
        {
            var auth = new SingleUserAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = Keys.TwitterConsumerKey,
                    ConsumerSecret = Keys.TwitterConsumerSecret,
                    OAuthToken = userDetails.Token,
                    OAuthTokenSecret = userDetails.TokenSecret,
                },
            };
            await auth.AuthorizeAsync();

            _ctx = new TwitterContext(auth);
        }

        public async Task<IList<StatusViewModel>> Load()
        {
            IList<StatusViewModel> newStatuses;

            var newestStatusId = _persistingManager.NewestStatusId;
            if (newestStatusId.HasValue)
            {
                newStatuses =
                await _ctx.Status
                    .Where(tweet => tweet.Type == StatusType.Home && tweet.SinceID == (ulong)newestStatusId.Value)
                    .Select(x => new StatusViewModel(x))
                    .ToListAsync();
            }
            else
            {
                newStatuses = await _ctx.Status
                    .Where(x => x.Type == StatusType.Home && x.Count == ChunkSize)
                    .Select(x => new StatusViewModel(x))
                    .ToListAsync();
            }

            return newStatuses;
        }
    }
}
