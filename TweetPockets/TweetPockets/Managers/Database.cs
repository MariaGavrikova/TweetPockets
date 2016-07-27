using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Interop;
using TweetPockets.ViewModels;

namespace TweetPockets.Managers
{
    public class Database : SQLiteConnection
    {
        public Database(ISQLitePlatform platform, string path) : base(platform, path)
        {
            CreateTable<StatusViewModel>();
            CreateTable<PhotoUrlViewModel>();
            CreateTable<ResourceUrlViewModel>();
        }
    }
}
