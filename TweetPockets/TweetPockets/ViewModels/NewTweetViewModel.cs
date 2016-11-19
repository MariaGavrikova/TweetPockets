using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TweetPockets.ViewModels
{
    public class NewTweetViewModel : ViewModelBase
    {
        public ICommand SendCommand { get; set; }
    }
}
