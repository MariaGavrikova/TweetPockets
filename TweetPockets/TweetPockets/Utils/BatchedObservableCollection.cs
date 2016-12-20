using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TweetPockets.ViewModels;

namespace TweetPockets.Utils
{
    public abstract class BatchedObservableCollection<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        private List<T> _list = new List<T>();

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
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, _list.Count - 1));
        }

        public void Insert(int i, T item)
        {
            _list.Insert(i, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, i));
        }

        public void AddRange(IList<T> items)
        {
            if (items.Count > 0)
            {
                var startingIndex = _list.Count;
                _list.AddRange(items);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, startingIndex));
            }
        }

        public void InsertRange(IList<T> items)
        {
            if (items.Count > 0)
            {
                _list.InsertRange(0, items);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, 0));
            }
        }

        public void ReplaceRange(IList<T> items)
        {
            var oldItems = _list;
            if (items.Count > 0)
            {
                _list = new List<T>();
                _list.AddRange(items);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, items, oldItems));
            }
        }

        public void Remove(T item)
        {
            var i = _list.IndexOf(item);
            _list.Remove(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }, i));
        }

        private void RemoveRange(int startingIndex, int count)
        {
            var oldItems = _list.GetRange(startingIndex, count);
            _list.RemoveRange(startingIndex, count);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, startingIndex));
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
    }
}
