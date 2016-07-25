using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TweetPockets.ViewModels;

namespace TweetPockets.Utils
{
    public class BatchedObservableCollection<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        private readonly List<T> _list = new List<T>();

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

        public void AddRange(List<T> items)
        {
            var startingIndex = _list.Count;
            _list.AddRange(items);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, startingIndex));
        }

        public void InsertRange(List<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                _list.Insert(i, items[i]);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, 0));
        }

        public void Remove(T item)
        {
            var i = _list.IndexOf(item);
            _list.Remove(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }, i));
        }

        public T this[int i]
        {
            get { return _list[i]; }
        }

        public int Count
        {
            get { return _list.Count; }
        }
    }
}
