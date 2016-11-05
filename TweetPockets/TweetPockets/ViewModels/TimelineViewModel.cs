using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using LinqToTwitter;
using TweetPockets.Managers;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class TimelineViewModel : MenuItemViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly TimelineManager _timelineManager;
        private bool _isLoadingNew;
        private bool _isLoadingOld;
        private RetweetsFavoritesManager _retweetsFavoritesManager;

        private const int TimelineLimit = 200;

        public TimelineViewModel(
            MainViewModel mainViewModel,
            StatusLoadingManager loadingManager,
            StatusPersistingManager persistingManager)
            : base(AppResources.TimelineMenuItem, "ic_book_black_24dp.png")
        {
            _mainViewModel = mainViewModel;
            _retweetsFavoritesManager = new RetweetsFavoritesManager(loadingManager, persistingManager);
            _timelineManager = new TimelineManager(loadingManager, persistingManager);
            LoadOldCommand = new Command(OnLoadOld);
            LoadNewCommand = new Command(OnLoadNew);
            MoveToReadLaterCommand = new Command(MoveToReadLater);
            FavoriteCommand = new Command(async (o) => await OnFavorite(o));
            Timeline = new BatchedObservableCollection<StatusViewModel>();
            _timelineManager.LoadingNewStarted += LoadingNewStartedHandler;
            _timelineManager.LoadingNewEnded += LoadingNewEndedHandler;
            _timelineManager.LoadedNewItems += LoadedNewItemsHandler;
            _timelineManager.LoadingOldStarted += LoadingOldStartedHandler;
            _timelineManager.LoadingOldEnded += LoadingOldEndedHandler;
            _timelineManager.LoadedOldItems += LoadedOldItemsHandler;
        }

        public bool IsLoadingNew
        {
            get { return _isLoadingNew; }
            set
            {
                _isLoadingNew = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoadingOld
        {
            get { return _isLoadingOld; }
            set
            {
                _isLoadingOld = value;
                OnPropertyChanged();
            }
        }

        public BatchedObservableCollection<StatusViewModel> Timeline { get; private set; }

        public ICommand LoadOldCommand { get; set; }

        public ICommand LoadNewCommand { get; set; }

        public ICommand MoveToReadLaterCommand { get; set; }

        public ICommand FavoriteCommand { get; set; }

        private void LoadingNewStartedHandler(object sender, EventArgs e)
        {
            IsLoadingNew = true;
        }

        private void LoadingNewEndedHandler(object sender, EventArgs e)
        {
            IsLoadingNew = false;
        }

        private void LoadedNewItemsHandler(object sender, NewItemsEventArgs e)
        {
            if (e.TooManyNewItems)
            {
                Timeline.ReplaceRange(e.Items);
            }
            else
            {
                Timeline.InsertRange(e.Items);

                if (Timeline.Count > TimelineLimit)
                {
                    Timeline.RemoveLast(TimelineLimit - Timeline.Count);
                }
            }
        }

        private void LoadingOldStartedHandler(object sender, EventArgs e)
        {
            IsLoadingOld = true;
        }

        private void LoadingOldEndedHandler(object sender, EventArgs e)
        {
            IsLoadingOld = false;
        }

        private void LoadedOldItemsHandler(object sender, ItemsEventArgs e)
        {
            Timeline.AddRange(e.Items);

            if (Timeline.Count > TimelineLimit)
            {
                Timeline.RemoveFirst(TimelineLimit - Timeline.Count);
            }
        }

        public async Task InitAsync()
        {
            Timeline.AddRange(await _timelineManager.GetCachedAsync());
            await _timelineManager.TriggerLoadingNew();
        }

        private async void OnLoadNew()
        {
            await _timelineManager.TriggerLoadingNew();
        }

        private async void OnLoadOld()
        {
            var last = Timeline.LastOrDefault();
            if (last != null)
            {
                await _timelineManager.TriggerLoadingOld(last.Id);
            }
        }

        private void MoveToReadLater(object obj)
        {
            var item = (StatusViewModel)obj;

            MessagingCenter.Send(_mainViewModel, "AddBookmark", item);
        }

        private async Task OnFavorite(object obj)
        {
            var item = (StatusViewModel)obj;
            item.IsFavorite = !item.IsFavorite;
            await _retweetsFavoritesManager.AddFavorite(item);
        }
    }
}
