using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Managers;
using TweetPockets.Managers.Authorization;
using TweetPockets.Utils;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        private readonly StatusLoadingManager _loadingManager;
        private UserInfoViewModel _user;
        private UserInfoPersistingManager _userInfoPersistingManager;

        public InfoViewModel(StatusLoadingManager loadingManager)
        {
            _loadingManager = loadingManager;
            _userInfoPersistingManager = new UserInfoPersistingManager();
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

        public async Task InitAsync()
        {
            var user = AuthorizationManager.Instance.CurrentUserDetails;
            User = _userInfoPersistingManager.GetCachedAsync((long)user.TwitterId);
            var userAccount = await _loadingManager.GetUserInfo(user.TwitterId, user.ScreenName);
            User = new UserInfoViewModel()
            {
                ScreenName = userAccount.ScreenNameResponse,
                Name = userAccount.Name,
                AvatarUrl = userAccount.ProfileImageUrl.Replace("_normal", "_bigger"),
                BannerUrl = userAccount.ProfileBannerUrl,
                BackgroundColorString = userAccount.ProfileBackgroundColor
            };
            _userInfoPersistingManager.Save(User);
        }
    }
}
