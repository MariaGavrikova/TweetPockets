using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Managers;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        private readonly StatusLoadingManager _loadingManager;
        private UserInfoViewModel _user;
        private InfoPersistingManager _infoPersistingManager;

        public InfoViewModel(StatusLoadingManager loadingManager)
        {
            _loadingManager = loadingManager;
            _infoPersistingManager = new InfoPersistingManager();
        }

        public UserInfoViewModel User
        {
            get { return _user; }
            set
            {
                _user = value; 
                OnPropertyChanged();
            }
        }

        public async Task InitAsync(UserDetails user)
        {
            User = _infoPersistingManager.GetCachedAsync((long)user.TwitterId);
            var userAccount = await _loadingManager.GetUserInfo(user.TwitterId, user.ScreenName);
            User = new UserInfoViewModel()
            {
                ScreenName = userAccount.ScreenNameResponse,
                Name = userAccount.Name,
                AvatarUrl = userAccount.ProfileImageUrl,
            };
            _infoPersistingManager.Save(User);
        }
    }
}
