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
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Managers
{
    public class StatusLoadingManager
    {
        private readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);
        private DateTime? _loadNewRequestTimestamp;
        private DateTime? _loadOldRequestTimestamp;

        public TwitterContext Context { get; private set; }

        public long? OldestStatusId { get; private set; }

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

            Context = new TwitterContext(auth);
        }

        public async Task<IList<StatusViewModel>> GetNewerThan(long minId, int count)
        {
            bool requestAllowed = true;
            if (_loadNewRequestTimestamp.HasValue)
            {
                requestAllowed = DateTime.UtcNow - _loadNewRequestTimestamp.Value < Timeout;
            }

            IList<StatusViewModel> newStatuses = new List<StatusViewModel>();
            if (requestAllowed)
            {
                _loadNewRequestTimestamp = DateTime.UtcNow;
                if (minId != 0)
                {
                    newStatuses =
                        await Context.Status
                            .Where(x => x.Type == StatusType.Home && x.SinceID == (ulong) minId && x.Count == count)
                            .Select(x => new StatusViewModel(x))
                            .ToListAsync();
                }
                else
                {
                    newStatuses = await Context.Status
                        .Where(x => x.Type == StatusType.Home && x.Count == count)
                        .Select(x => new StatusViewModel(x))
                        .ToListAsync();
                }
            }

            return newStatuses;
        }

        public async Task<IList<StatusViewModel>> GetOlderThan(long maxId, int count)
        {
            if (_loadOldRequestTimestamp.HasValue)
            {
                var currentTimeout = DateTime.UtcNow - _loadOldRequestTimestamp.Value;
                if (currentTimeout < Timeout)
                {
                    await Task.Delay(Timeout - currentTimeout);
                }
            }

            _loadOldRequestTimestamp = DateTime.UtcNow;

            IList<StatusViewModel> oldStatuses = await Context.Status
                    .Where(x => x.Type == StatusType.Home && x.MaxID == (ulong)maxId && x.Count == count)
                    .Select(x => new StatusViewModel(x))
                    .ToListAsync();
            if (oldStatuses.Any() && oldStatuses[0].Id == maxId)
            {
                oldStatuses.RemoveAt(0);
            }
            return oldStatuses;
        }

        public async Task<User> GetUserInfo(ulong id, string screenName)
        {
            var user =
                 await
                 (from tweet in Context.User
                  where tweet.Type == UserType.Show &&
                        tweet.UserID == id
                  select tweet)
                 .SingleOrDefaultAsync();

            return user;
        }

        public async Task AddFavorite(ulong id, bool isFavorite)
        {
            if (isFavorite)
            {
                await Context.CreateFavoriteAsync(id);
            }
            else
            {
                await Context.DestroyFavoriteAsync(id);
            }
        }
    }
}
