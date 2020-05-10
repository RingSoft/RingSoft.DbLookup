using System.Collections;
using System.Collections.Generic;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupColumnEnumerator : IEnumerator<LookupColumn>
    {
        private LookupColumnCollection _collection;
        private int _curIndex = -1;
        private LookupColumn _currentLookupColumn;

        public LookupColumnEnumerator(LookupColumnCollection collection)
        {
            _collection = collection;
            _currentLookupColumn = default(LookupColumn);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++_curIndex >= _collection.Count)
            {
                return false;
            }

            // Set current box to next item in collection.
            _currentLookupColumn = _collection[_curIndex];
            return true;
        }

        public void Reset()
        {
            _curIndex = -1;
        }

        public LookupColumn Current => _currentLookupColumn;

        object IEnumerator.Current => Current;
    }
}
