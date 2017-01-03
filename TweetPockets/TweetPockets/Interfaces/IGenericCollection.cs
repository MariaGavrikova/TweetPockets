using System;

namespace TweetPockets.Interfaces
{
    public interface IGenericCollection
    {
        Type ItemType { get; }
    }
}