using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(SQLitePlatformFactory))]

namespace TweetPockets.Droid.PlatformSpecificCode
{
    class SQLitePlatformFactory : ISQLitePlatformFactory
    {
        public ISQLitePlatform CreatePlatform()
        {
            return new SQLitePlatformAndroid();
        }

        public string CreateDatabaseFile(string fileName)
        {
            var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbPath = System.IO.Path.Combine(folder, fileName);
            if (!System.IO.File.Exists(dbPath))
            {
                using (var f = System.IO.File.Create(dbPath)) { }
            }

            return dbPath;
        }
    }
}