using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Utils;
using TweetPockets.ViewModels;

namespace TweetPockets.Managers
{
    class TimelineManager
    {
        private readonly StatusPersistingManager _persistingManager;
        private readonly StatusLoadingManager _loadingManager;
       
        private const int ItemsChunk = 20;

        public TimelineManager()
        {
            _persistingManager = new StatusPersistingManager();
            _loadingManager = new StatusLoadingManager();
        }

        public event EventHandler LoadingNewStarted;

        public event EventHandler LoadingNewEnded;

        public event EventHandler<NewItemsEventArgs> LoadedNewItems;

        public event EventHandler LoadingOldStarted;

        public event EventHandler LoadingOldEnded;

        public event EventHandler<ItemsEventArgs> LoadedOldItems;

        public async Task Init(UserDetails userDetails)
        {
            OnLoadingStarted();

            await _loadingManager.Init(userDetails);
        }

        public Task<IList<StatusViewModel>> GetCachedAsync()
        {
            return Task.FromResult(_persistingManager.GetMostRecent(ItemsChunk));
        }

        public async Task TriggerLoadingNew()
        {
            OnLoadingStarted();

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

                OnLoadingEnded();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task TriggerLoadingOld(long maxId)
        {
            OnLoadingOldStarted();

            var items = await _loadingManager.GetOlderThan(maxId, ItemsChunk);

            OnLoadedOldItems(new ItemsEventArgs()
            {
                Items = items
            });

            OnLoadingOldEnded();
        }

        protected virtual void OnLoadingStarted()
        {
            LoadingNewStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadingEnded()
        {
            LoadingNewEnded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadedNewItems(NewItemsEventArgs e)
        {
            LoadedNewItems?.Invoke(this, e);
        }

        protected virtual void OnLoadingOldStarted()
        {
            LoadingOldStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadingOldEnded()
        {
            LoadingOldEnded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadedOldItems(ItemsEventArgs e)
        {
            LoadedOldItems?.Invoke(this, e);
        }
    }
}
