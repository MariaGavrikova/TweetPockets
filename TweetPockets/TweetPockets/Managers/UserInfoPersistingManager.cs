using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;
using TweetPockets.Interfaces;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    public class UserInfoPersistingManager
    {
        private const string DatabaseFileName = "data.db";
        private readonly Database _db;

        public UserInfoPersistingManager()
        {
            var platformFactory = DependencyService.Get<ISQLitePlatformFactory>();
            _db = new Database(platformFactory.CreatePlatform(), platformFactory.CreateDatabaseFile(DatabaseFileName));
        }

        public UserInfoViewModel GetCachedAsync(long userId)
        {
            var info =
                _db.Find<UserInfoViewModel>(x => x.Id == userId);
            return info;
        }

        public void Save(UserInfoViewModel user)
        {
            _db.InsertOrReplace(user);
        }
    }
}
