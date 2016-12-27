using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Managers
{
    public class StatusLoadingManager
    {
        private readonly BookmarkPersistingManager _persistingManager;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);
        private DateTime? _loadNewRequestTimestamp;
        private DateTime? _loadOldRequestTimestamp;

        public StatusLoadingManager(BookmarkPersistingManager persistingManager)
        {
            _persistingManager = persistingManager;
        }

        protected TwitterContext Context { get; private set; }

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

        public async Task<IList<ITimelineEntity>> GetNewerThan(long minId, int count)
        {
            try
            {
                bool requestAllowed = true;
                if (_loadNewRequestTimestamp.HasValue)
                {
                    requestAllowed = DateTime.UtcNow - _loadNewRequestTimestamp.Value >= _timeout;
                }

                IList<ITimelineEntity> newStatuses = new List<ITimelineEntity>();
                if (requestAllowed)
                {
                    _loadNewRequestTimestamp = DateTime.UtcNow;
                    if (minId != 0)
                    {
                        newStatuses =
                            await Context.Status
                                .Where(x => x.Type == StatusType.Home && x.SinceID == (ulong)minId && x.Count == count)
                                .Select(x => new StatusViewModel(x, _persistingManager))
                                .ToListAsync<ITimelineEntity>();
                    }
                    else
                    {
                        newStatuses = await Context.Status
                            .Where(x => x.Type == StatusType.Home && x.Count == count)
                            .Select(x => new StatusViewModel(x, _persistingManager))
                            .ToListAsync<ITimelineEntity>();
                    }
                }

                return newStatuses;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IList<ITimelineEntity>> GetOlderThan(long maxId, int count)
        {
            if (_loadOldRequestTimestamp.HasValue)
            {
                var currentTimeout = DateTime.UtcNow - _loadOldRequestTimestamp.Value;
                if (currentTimeout < _timeout)
                {
                    await Task.Delay(_timeout - currentTimeout);
                }
            }

            _loadOldRequestTimestamp = DateTime.UtcNow;

            IList<ITimelineEntity> oldStatuses = await Context.Status
                    .Where(x => x.Type == StatusType.Home && x.MaxID == (ulong)maxId && x.Count == count)
                    .Select(x => new StatusViewModel(x, _persistingManager))
                    .ToListAsync<ITimelineEntity>();
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

        public async Task<Status> AddRetweet(ulong id, bool retweet)
        {
            Status result = null;
            if (retweet)
            {
                result = await Context.RetweetAsync(id);
            }
            else
            {
                try
                {
                    await Context.DeleteTweetAsync(id);
                }
                catch (TwitterQueryException ex)
                {
                }
            }

            return result;
        }

        public async Task<Status> AddStatus(string text, StatusViewModel inReplyToTweet)
        {
            if (inReplyToTweet != null)
            {
                return await Context.ReplyAsync((ulong) inReplyToTweet.Id, text);
            }
            return await Context.TweetAsync(text);
        }
    }
}
