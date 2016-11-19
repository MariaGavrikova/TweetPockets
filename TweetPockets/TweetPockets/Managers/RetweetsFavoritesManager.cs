﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Managers
{
    public class RetweetsFavoritesManager
    {
        private readonly StatusLoadingManager _loadingManager;
        private readonly StatusPersistingManager _persistingManager;

        public RetweetsFavoritesManager(StatusLoadingManager loadingManager, StatusPersistingManager persistingManager)
        {
            this._loadingManager = loadingManager;
            _persistingManager = persistingManager;
        }

        public async Task AddFavorite(StatusViewModel status)
        {
            await _loadingManager.AddFavorite((ulong)status.Id, status.IsFavorite);
            _persistingManager.Save(status);
        }

        public async Task AddRetweet(StatusViewModel status)
        {
            var newStatus = await _loadingManager.AddRetweet((ulong)status.Id, status.IsRetweeted);
            if (newStatus != null)
            {
                _persistingManager.SaveWithNewId(status, (long)newStatus.StatusID);
            }
            else
            {
                _persistingManager.SaveWithNewId(status, status.OldId.GetValueOrDefault());
            }
        }
    }
}
