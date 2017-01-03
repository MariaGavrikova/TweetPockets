namespace TweetPockets.ViewModels.Entities
{
    public enum UserStreamEventType
    {
        Unknown,
        Follow,
        UserUpdate,
        Favorite,
        Unfavorite,
        Retweet,
        Quoted,
        Replied,
        Block,
        Unblock,
        ListCreated,
        ListDestroyed,
        ListUpdated,
        ListMemberAdded,
        ListMemberRemoved,
        ListUserSubscribed,
        ListUserUnsubscribed,
    }
}