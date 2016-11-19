using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TweetPockets.Managers;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class NewTweetViewModel : ViewModelBase
    {
        private readonly TweetActionsManager _actionsManager;
        private readonly TimelineViewModel _timelineViewModel;
        private string _text;

        public NewTweetViewModel(TweetActionsManager actionsManager, TimelineViewModel timelineViewModel)
        {
            _actionsManager = actionsManager;
            _timelineViewModel = timelineViewModel;
            SendCommand = new Command(async () => await OnSend());
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool IsValidLength { get; set; }

        public ICommand SendCommand { get; set; }

        private async Task OnSend()
        {
            if (!String.IsNullOrWhiteSpace(Text))
            {
                await _actionsManager.AddNewStatus(Text);
                await App.Instance.MainPage.Navigation.PopAsync();
                _timelineViewModel.OnLoadNew();
            }
        }
    }
}
