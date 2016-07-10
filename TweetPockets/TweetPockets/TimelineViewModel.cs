using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LinqToTwitter;
using Xamarin.Forms;

namespace TweetPockets
{
    public class TimelineViewModel : PageViewModel
    {
        private TwitterContext _ctx;
        private int ChunkSize = 20;
        private ulong _oldestStatusId;
        private bool _isLoading;
        private ulong _newestStatusId;
        private StatusViewModelFactory _factory;

        public TimelineViewModel()
        {
            LoadOldCommand = new Command(OnLoadOld);
            LoadNewCommand = new Command(OnLoadNew);
            MoveToReadLaterCommand = new Command(MoveToReadLater);
            _factory = new StatusViewModelFactory();
            Timeline = new BatchedObservableCollection<StatusViewModel>();
            Bookmarks = new BatchedObservableCollection<StatusViewModel>();
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public BatchedObservableCollection<StatusViewModel> Timeline { get; private set; }

        public BatchedObservableCollection<StatusViewModel> Bookmarks { get; private set; }

        public ICommand LoadOldCommand { get; set; }

        public ICommand LoadNewCommand { get; set; }

        public ICommand MoveToReadLaterCommand { get; set; }

        public async Task InitAsync(UserDetails userDetails)
        {
            try
            {
                IsLoading = true;

                var auth = new SingleUserAuthorizer()
                {
                    CredentialStore = new InMemoryCredentialStore()
                    {
                        ConsumerKey = Keys.TwitterConsumerKey,
                        ConsumerSecret = Keys.TwitterConsumerSecret,
                        OAuthToken = userDetails.Token,
                        OAuthTokenSecret = userDetails.TokenSecret,
                        ScreenName = userDetails.ScreenName,
                        UserID = ulong.Parse(userDetails.TwitterId)
                    },
                };
                await auth.AuthorizeAsync();

                _ctx = new TwitterContext(auth);

                Timeline.AddRange(
                 await
                 (from tweet in _ctx.Status
                  where tweet.Type == StatusType.Home && tweet.Count == ChunkSize
                  select _factory.Create(tweet))
                 .ToListAsync());

                UpdateNewestStatusId();
                UpdateOldestStatusId();

                IsLoading = false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void UpdateNewestStatusId()
        {
            if (Timeline.Any())
            {
                _newestStatusId = Timeline[0].Id;
            }
        }

        private void UpdateOldestStatusId()
        {
            if (Timeline.Any())
            {
                _oldestStatusId = Timeline[Timeline.Count - 1].Id;
            }
        }

        private async void OnLoadOld()
        {
            var oldStatuses =
                await
             (from tweet in _ctx.Status
              where tweet.Type == StatusType.Home && tweet.Count == ChunkSize && tweet.MaxID == _oldestStatusId
              select new StatusViewModel(tweet))
             .ToListAsync();

            foreach (var oldStatus in oldStatuses)
            {
                Timeline.Add(oldStatus);
            }

            UpdateOldestStatusId();
        }

        private async void OnLoadNew()
        {
            IsLoading = true;

            var newStatuses =
                await
             (from tweet in _ctx.Status
              where tweet.Type == StatusType.Home && tweet.SinceID == _newestStatusId
              select new StatusViewModel(tweet))
             .ToListAsync();

            for (int i = 0; i < newStatuses.Count; i++)
            {
                Timeline.Insert(i, newStatuses[i]);
            }

            UpdateNewestStatusId();

            IsLoading = false;
        }

        private void MoveToReadLater(object obj)
        {
            var statusIndex = (int)obj;
            var item = Timeline[statusIndex];
            Timeline.RemoveAt(statusIndex);
            Bookmarks.Add(item);


        }
    }
}
