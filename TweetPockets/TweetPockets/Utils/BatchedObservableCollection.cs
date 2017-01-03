using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TweetPockets.Interfaces;
using TweetPockets.Interfaces.Entities;
using TweetPockets.ViewModels;
using TweetPockets.ViewModels.Entities;

namespace TweetPockets.Utils
{
    public abstract class BatchedObservableCollection<T> : IEnumerable<T>, INotifyCollectionChanged, IGenericCollection
        where T : IEntity
    {
        private List<T> _list = new List<T>();
        private Dictionary<long, T> _dictionary = new Dictionary<long, T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _list.Add(item);
            _dictionary[item.Id] = item;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, _list.Count - 1));
        }

        public void Insert(int i, T item)
        {
            _list.Insert(i, item);
            _dictionary[item.Id] = item;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, i));
        }

        public void AddRange(IEnumerable<T> items)
        {
            var startingIndex = _list.Count;
            var newItems = new List<T>();
            foreach (var item in items)
            {
                newItems.Add(item);
                _list.Add(item);
                _dictionary[item.Id] = item;
            }
            if (newItems.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, startingIndex));
            }
        }

        public void InsertRange(IList<T> items)
        {
            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    _list.Insert(i, item);
                    _dictionary[item.Id] = item;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, 0));
            }
        }

        public void ReplaceRange(IEnumerable<T> items)
        {
            var oldItems = _list;
            _list = new List<T>();
            _dictionary = new Dictionary<long, T>();
            foreach (var item in items)
            {
                _list.Add(item);
                _dictionary[item.Id] = item;
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, items, oldItems));
        }

        public void Remove(T item)
        {
            var i = _list.IndexOf(item);
            _list.Remove(item);
            _dictionary.Remove(item.Id);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }, i));
        }

        private void RemoveRange(int startingIndex, int count)
        {
            var oldItems = new List<T>();
            for (int i = startingIndex; i < startingIndex + count; i++)
            {
                var item = _list[i];
                _dictionary.Remove(item.Id);
                _list.RemoveAt(i);
                oldItems.Add(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, startingIndex));
        }

        public void Clear()
        {
            if (_list.Any())
            {
                _dictionary.Clear();
                _list.Clear();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void RemoveLast(int count)
        {
            RemoveRange(Count - count, count);
        }

        public void RemoveFirst(int count)
        {
            RemoveRange(0, count);
        }

        public T this[int i]
        {
            get { return _list[i]; }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public abstract bool HasMoreItems { get; }

        public T GetById(long id)
        {
            T result;
            _dictionary.TryGetValue(id, out result);
            return result;
        }

        public Type ItemType
        {
            get { return typeof (T); }
        }
    }
}
