﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Managers
{
    public class TweetActionsManager
    {
        private readonly StatusLoadingManager _loadingManager;
        private readonly StatusPersistingManager _persistingManager;
        private readonly BookmarkPersistingManager _bookmarkPersistingManager;

        public TweetActionsManager(
            StatusLoadingManager loadingManager,
            StatusPersistingManager persistingManager,
            BookmarkPersistingManager bookmarkPersistingManager)
        {
            this._loadingManager = loadingManager;
            _persistingManager = persistingManager;
            _bookmarkPersistingManager = bookmarkPersistingManager;
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

        public async Task AddNewStatus(string text, StatusViewModel inReplyToTweet)
        {
            var statusModel = await _loadingManager.AddStatus(text, inReplyToTweet);
            var status = new StatusViewModel(statusModel, _bookmarkPersistingManager);
            _persistingManager.Save(status);
        }
    }
}
