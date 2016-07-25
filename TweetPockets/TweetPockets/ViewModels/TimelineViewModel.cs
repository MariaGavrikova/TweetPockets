using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using LinqToTwitter;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class TimelineViewModel : PageViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private TwitterContext _ctx;
        private int ChunkSize = 20;
        private ulong _oldestStatusId;
        private bool _isLoading;
        private ulong _newestStatusId;
        private StatusViewModelFactory _factory;

        public TimelineViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoadOldCommand = new Command(OnLoadOld);
            LoadNewCommand = new Command(OnLoadNew);
            MoveToReadLaterCommand = new Command(MoveToReadLater);
            _factory = new StatusViewModelFactory();
            Timeline = new BatchedObservableCollection<StatusViewModel>();
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
                    },
                };
                await auth.AuthorizeAsync();

                _ctx = new TwitterContext(auth);

                Timeline.AddRange(
                    await _ctx.Status
                        .Where(x => x.Type == StatusType.Home && x.Count == ChunkSize)
                        .Select(x => _factory.Create(x))
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
        }

        private async void OnLoadNew()
        {
            IsLoading = true;

            var newStatuses =
                await _ctx.Status
                    .Where(tweet => tweet.Type == StatusType.Home && tweet.SinceID == _newestStatusId)
                    .Select(x => _factory.Create(x))
                    .ToListAsync();

            Timeline.InsertRange(newStatuses);

            UpdateNewestStatusId();

            IsLoading = false;
        }

        private void MoveToReadLater(object obj)
        {
            var item = (StatusViewModel) obj;

            MessagingCenter.Send(_mainViewModel, "AddBookmark", item);
        }
    }
}
