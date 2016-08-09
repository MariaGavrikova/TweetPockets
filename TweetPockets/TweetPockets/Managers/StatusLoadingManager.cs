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
        private TwitterContext _ctx;
        private object _syncRoot = new object();
        private DateTime? _timestamp;

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

            _ctx = new TwitterContext(auth);
        }

        public async Task<IList<StatusViewModel>> GetNewerThan(long minId, int count)
        {
            bool allowRequest = true;
            lock (_syncRoot)
            {
                if (!_timestamp.HasValue)
                {
                    _timestamp = DateTime.UtcNow;
                }
                else
                {
                    allowRequest = DateTime.UtcNow - _timestamp > TimeSpan.FromSeconds(30);
                }
            }

            IList<StatusViewModel> newStatuses = new List<StatusViewModel>();
            if (allowRequest)
            {
                if (minId != 0)
                {
                    newStatuses =
                        await _ctx.Status
                            .Where(x => x.Type == StatusType.Home && x.SinceID == (ulong)minId && x.Count == count)
                            .Select(x => new StatusViewModel(x))
                            .ToListAsync();
                }
                else
                {
                    newStatuses = await _ctx.Status
                        .Where(x => x.Type == StatusType.Home && x.Count == count)
                        .Select(x => new StatusViewModel(x))
                        .ToListAsync();
                }
            }

            return newStatuses;
        }

        public async Task<IList<StatusViewModel>> GetOlderThan(long maxId, int count)
        {
            var oldStatuses = await _ctx.Status
                            .Where(x => x.Type == StatusType.Home && x.MaxID == (ulong)maxId && x.Count == count)
                            .Select(x => new StatusViewModel(x))
                            .ToListAsync();
            return oldStatuses;
        }
    }
}
