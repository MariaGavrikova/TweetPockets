using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

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
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }));
        }

        public void Insert(int i, T item)
        {
            _list.Insert(i, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }, i));
        }

        public void AddRange(IEnumerable<T> items)
        {
            _list.AddRange(items);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void RemoveAt(int i)
        {
            var item = this[i];
            _list.RemoveAt(i);
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
