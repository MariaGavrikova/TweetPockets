using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using LinqToTwitter;
using TweetPockets.Managers;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class TimelineViewModel : PageViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly StatusPersistingManager _persistingManager;
        private readonly StatusLoadingManager _loadingManager;
        private bool _isLoading;
        
        public TimelineViewModel(MainViewModel mainViewModel,
            StatusPersistingManager persistingManager,
            StatusLoadingManager loadingManager)
        {
            _mainViewModel = mainViewModel;
            _persistingManager = persistingManager;
            _loadingManager = loadingManager;
            LoadOldCommand = new Command(OnLoadOld);
            LoadNewCommand = new Command(OnLoadNew);
            MoveToReadLaterCommand = new Command(MoveToReadLater);
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

                await _loadingManager.Init(userDetails);

                var oldStatuses = _persistingManager.Load();
                Timeline.AddRange(oldStatuses);

                var newStatuses = await _loadingManager.Load();
                Timeline.InsertRange(newStatuses);
                _persistingManager.Save(newStatuses);

                IsLoading = false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async void OnLoadOld()
        {
        }

        private async void OnLoadNew()
        {
            IsLoading = true;

            var newStatuses = await _loadingManager.Load();
            Timeline.InsertRange(newStatuses);
            _persistingManager.Save(newStatuses);

            IsLoading = false;
        }

        private void MoveToReadLater(object obj)
        {
            var item = (StatusViewModel) obj;

            MessagingCenter.Send(_mainViewModel, "AddBookmark", item);
        }
    }
}
