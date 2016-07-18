﻿using System.Linq;
using LinqToTwitter;

namespace TweetPockets.ViewModels
{
    public class StatusViewModel : ViewModelBase
    {
        public StatusViewModel(Status model, int i)
        {
            Id = model.StatusID;
            Index = i;
            Author = model.User.Name;
            AuthorImageUrl = model.User.ProfileImageUrl;
            Text = model.Text;

            CanBeReadLater = model.Entities.UrlEntities.Any();
        }

        public int Index { get; set; }

        public ulong Id { get; set; }

        public string Author { get; set; }

        public string AuthorImageUrl { get; set; }

        public string Text { get; set; }

        public bool CanBeReadLater { get; set; }
    }
}