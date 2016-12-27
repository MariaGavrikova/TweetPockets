using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TweetPockets.Managers;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class NewTweetViewModel : ViewModelBase
    {
        private readonly TweetActionsManager _actionsManager;
        private readonly TimelineViewModel _timelineViewModel;
        private readonly StatusViewModel _inReplyToTweet;
        private string _text;

        public NewTweetViewModel(
            TweetActionsManager actionsManager,
            TimelineViewModel timelineViewModel,
            StatusViewModel inReplyToTweet = null)
        {
            _actionsManager = actionsManager;
            _timelineViewModel = timelineViewModel;
            _inReplyToTweet = inReplyToTweet;

            if (inReplyToTweet == null)
            {
                Text = String.Empty;
            }
            else
            {
                Text = GetAuthorMention(inReplyToTweet);
            }

            SendCommand = new Command(async () => await OnSend());
        }

        private static string GetAuthorMention(StatusViewModel inReplyToTweet)
        {
            return String.Format("@{0} ", inReplyToTweet.AuthorScreenName);
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
            if (!String.IsNullOrWhiteSpace(Text) && 
                (_inReplyToTweet != null && Text != GetAuthorMention(_inReplyToTweet)))
            {
                await _actionsManager.AddNewStatus(Text, _inReplyToTweet);
                App.Instance.PopAsync();
                _timelineViewModel.OnLoadNew();
            }
        }
    }
}
