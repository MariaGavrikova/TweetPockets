﻿using System;
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
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    public class StatusPersistingManager
    {
        private const string DatabaseFileName = "data.db";
        private const int TotalItemsLimit = 30;
        private readonly Database _db;

        public StatusPersistingManager()
        {
            var platformFactory = DependencyService.Get<ISQLitePlatformFactory>();
            _db = new Database(platformFactory.CreatePlatform(), platformFactory.CreateDatabaseFile(DatabaseFileName));
        }

        public long NewestStatusId { get; private set; }

        public long ControlStatusId { get; private set; }

        public void Save(IList<ITimelineEntity> statuses)
        {
            if (statuses != null && statuses.Any())
            {
                if (statuses.Count == 1)
                {
                    _db.Insert(statuses[0]);
                    ControlStatusId = NewestStatusId;
                    NewestStatusId = statuses[0].Id;
                }
                else
                {
                    _db.InsertAllWithChildren(statuses);
                    NewestStatusId = statuses[0].Id;
                    ControlStatusId = statuses[1].Id;
                }

                var statusesTable = _db.Table<StatusViewModel>();
                var totalCount = statusesTable.Count();

                if (totalCount > TotalItemsLimit)
                {
                    _db.DeleteAll(statusesTable.OrderBy(x => x.CreatedAt).Take(totalCount - TotalItemsLimit), true);
                }
            }
        }

        public IList<ITimelineEntity> GetMostRecent(int count)
        {
            var items =
                _db.GetAllWithChildren<StatusViewModel>()
                .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .Cast<ITimelineEntity>()
                .ToList();

            if (items.Any())
            {
                NewestStatusId = items[0].Id;
                if (items.Count > 1)
                {
                    ControlStatusId = items[1].Id;
                }
            }
            return items;
        }

        public void RemoveAll()
        {
            _db.DeleteAll<StatusViewModel>();
        }

        public void Save(StatusViewModel status)
        {
            if (_db.Find<StatusViewModel>(status.Id) != null)
            {
                _db.Update(status);
            }
        }

        public void SaveWithNewId(StatusViewModel status, long statusId)
        {
            status.OldId = status.Id;
            status.Id = (long) statusId;

            var oldId = status.OldId.GetValueOrDefault();
            var oldStatus = _db.Find<StatusViewModel>(oldId);
            if (oldStatus != null)
            {
                _db.Delete(oldStatus);
                _db.Insert(status);
            }
        }
    }
}
