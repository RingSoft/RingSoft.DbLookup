using System;
using System.Collections;
using System.Collections.Generic;

namespace RingSoft.DbLookup.Controls.WPF
{
    public sealed class LookupColumnCollection : IList<LookupColumn>
    {
        public event EventHandler CollectionChanged;

        private List<LookupColumn> _lookupColumns = new List<LookupColumn>();

        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<LookupColumn> GetEnumerator()
        {
            return new LookupColumnEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LookupColumnEnumerator(this);
        }

        public void Add(LookupColumn item)
        {
            if (!Contains(item))
                _lookupColumns.Add(item);
            OnCollectionChanged();
        }

        public void Clear()
        {
            _lookupColumns.Clear();
            OnCollectionChanged();
        }

        public bool Contains(LookupColumn item)
        {
            return _lookupColumns.Contains(item);
        }

        public void CopyTo(LookupColumn[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), @"The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), @"The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < _lookupColumns.Count; i++)
            {
                array[i + arrayIndex] = _lookupColumns[i];
            }
            OnCollectionChanged();
        }

        public bool Remove(LookupColumn item)
        {
            var result = _lookupColumns.Remove(item);
            OnCollectionChanged();
            return result;
        }

        public int Count => _lookupColumns.Count;

        int ICollection<LookupColumn>.Count => Count;

        public bool IsReadOnly => false;

        public int IndexOf(LookupColumn item)
        {
            return _lookupColumns.IndexOf(item);
        }

        public void Insert(int index, LookupColumn item)
        {
            _lookupColumns.Insert(index, item);
            OnCollectionChanged();
        }

        void IList<LookupColumn>.RemoveAt(int index)
        {
            _lookupColumns.RemoveAt(index);
            OnCollectionChanged();
        }

        public LookupColumn this[int index]
        {
            get => _lookupColumns[index];
            set
            {
                _lookupColumns[index] = value;
                OnCollectionChanged();
            }
        }
    }
}
