using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Interfaces;
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    class TimelineManager
    {
        private readonly StatusPersistingManager _persistingManager;
        private readonly StatusLoadingManager _loadingManager;
        private bool _loadingNewStarted;
        private bool _loadingOldStarted;
        private ITimer _timer;

        private const int ItemsChunk = 20;

        public TimelineManager(StatusLoadingManager loadingManager, StatusPersistingManager persistingManager)
        {
            _persistingManager = persistingManager;
            _loadingManager = loadingManager;
            _timer = DependencyService.Get<ITimer>();
        }

        public event EventHandler LoadingNewStarted;

        public event EventHandler LoadingNewEnded;

        public event EventHandler<NewItemsEventArgs> LoadedNewItems;

        public event EventHandler LoadingOldStarted;

        public event EventHandler LoadingOldEnded;

        public event EventHandler<ItemsEventArgs> LoadedOldItems;

        public bool NotifyAboutNewItems { get; set; }

        private async Task<bool> OnTimerTick()
        {
            if (NotifyAboutNewItems)
            {
                await TriggerLoadingNew();
            }
            return true;
        }

        public Task<IList<ITimelineEntity>> InitAsync()
        {
            var tcs = new TaskCompletionSource<IList<ITimelineEntity>>();
            _timer.Interval = _loadingManager.Timeout;
            _timer.Tick += async (sender, args) =>
            {
                await OnTimerTick();
            };
            _timer.Start();
            NotifyAboutNewItems = true;
            try
            {
                var result = _persistingManager.GetMostRecent(ItemsChunk);
                tcs.SetResult(result);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
            return tcs.Task;
        }

        public async Task TriggerLoadingNew()
        {
            if (_loadingNewStarted) return;

            OnLoadingNewStarted();

            var controlStatusId = _persistingManager.ControlStatusId;
            try
            {
                var newItems = await _loadingManager.GetNewerThan(controlStatusId, ItemsChunk);
                var newestStatusId = _persistingManager.NewestStatusId;
                if (newItems.Any())
                {
                    var last = newItems.Count - 1;
                    bool tooManyItems = newItems[last].Id != newestStatusId;
                   
                    if (tooManyItems)
                    {
                        _persistingManager.RemoveAll();
                    }
                    else
                    {
                        newItems.RemoveAt(last);                        
                    }

                    _persistingManager.Save(newItems);

                    OnLoadedNewItems(new NewItemsEventArgs()
                    {
                        TooManyNewItems = tooManyItems,
                        Items = newItems
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            OnLoadingNewEnded();
        }

        public async Task TriggerLoadingOld(long maxId)
        {
            if (_loadingOldStarted) return;

            OnLoadingOldStarted();

            var items = await _loadingManager.GetOlderThan(maxId, ItemsChunk);

            OnLoadedOldItems(new ItemsEventArgs()
            {
                Items = items
            });

            OnLoadingOldEnded();
        }

        protected virtual void OnLoadingNewStarted()
        {
            _loadingNewStarted = true;
            LoadingNewStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadingNewEnded()
        {
            _loadingNewStarted = false;
            LoadingNewEnded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadedNewItems(NewItemsEventArgs e)
        {
            LoadedNewItems?.Invoke(this, e);
        }

        protected virtual void OnLoadingOldStarted()
        {
            _loadingOldStarted = true;
            LoadingOldStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadingOldEnded()
        {
            _loadingOldStarted = false;
            LoadingOldEnded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadedOldItems(ItemsEventArgs e)
        {
            LoadedOldItems?.Invoke(this, e);
        }
    }
}
