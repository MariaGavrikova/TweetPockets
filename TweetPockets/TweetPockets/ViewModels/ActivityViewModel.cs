using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Managers;
using TweetPockets.Managers.Notifications;
using TweetPockets.Resources;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Collections;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class ActivityViewModel : MenuItemViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly EventPersistingManager _persistingManager;

        public ActivityViewModel(MainViewModel mainViewModel, EventPersistingManager persistingManager)
            : base(AppResources.AcitivityMenuItem, "ic_bell_black_24dp.png", "ic_bell_green_24dp.png")
        {
            _mainViewModel = mainViewModel;
            _persistingManager = persistingManager;
            Events = new Events();
        }

        public BatchedObservableCollection<EventViewModel> Events { get; private set; }

        public override async Task Reload()
        {
            Events.ReplaceRange(await _persistingManager.GetEventsAsync());
        }
    }
}

