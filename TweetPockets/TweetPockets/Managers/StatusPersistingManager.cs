using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;
using TweetPockets.Interfaces;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    public class StatusPersistingManager
    {
        private int ChunkSize = 20;
        private const string DatabaseFileName = "data.db";
        private readonly Database _db;

        public StatusPersistingManager()
        {
            var platformFactory = DependencyService.Get<ISQLitePlatformFactory>();
            _db = new Database(platformFactory.CreatePlatform(), platformFactory.CreateDatabaseFile(DatabaseFileName));
        }

        public long? NewestStatusId { get; private set; }

        public IList<StatusViewModel> Load()
        {
            var list = _db.GetAllWithChildren<StatusViewModel>().OrderByDescending(x => x.CreatedAt).Take(ChunkSize).ToList();
            var firstItem = list.FirstOrDefault();
            if (firstItem != null)
            {
                NewestStatusId = firstItem.Id;
            }

            return list;
        }

        public void Save(IList<StatusViewModel> statuses)
        {
            _db.InsertAllWithChildren(statuses);

            var firstItem = statuses.FirstOrDefault();
            if (firstItem != null)
            {
                NewestStatusId = firstItem.Id;
            }
        }
    }
}
