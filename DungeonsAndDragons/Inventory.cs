using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonsAndDragons
{
    class Inventory<T> where T : Item, IComparable<T>
    {
        private SortedList<T, int> _contents;


        public Inventory() {
            _contents = new SortedList<T, int>();
        }


        /// <summary>
        /// Adds <paramref name="count"/> of <paramref name="item"/> to inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="count">The count of <paramref name="item"/> to add.</param>
        /// <returns><c>true</c> if the addition was successful, <c>false</c> otherwise.</returns>
        public bool AddItem(T item, int count)
        {
            if (count == 0) { return true; }
            else if (count < 0) { return false; }
            else if ((item.CountMax >= 0) && (count > item.CountMax)) { return false; }

            if (!_contents.ContainsKey(item)) { _contents[item] = 0; }
            _contents[item] += count;

            return true;
        }

        /// <summary>
        /// Removes <paramref name="count"/> of <paramref name="item"/> from inventory.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <param name="count">The count of <paramref name="item"/> to remove.</param>
        /// <returns><c>true</c> if the removal was successful, <c>false</c> otherwise.</returns>
        public bool RemoveItem(T item, int count)
        {
            if (!_contents.TryGetValue(item, out int countPresent) || (countPresent < count)) { return false; }
            else if (countPresent == count) { return _contents.Remove(item); }
            else
            {
                _contents[item] -= count;

                return true;
            }
        }

        /// <summary>
        /// Removes all of <paramref name="item"/> from inventor.
        /// </summary>
        /// <param name="item" cref="RemoveItem(T)"/>
        /// <returns cref="RemoveItem(T)"/>
        public bool RemoveItem(T item)
        {
            if (!_contents.TryGetValue(item, out int count)) { return false; }
            
            return RemoveItem(item, count);
        }

        public void Print(TextWriter textWriter)
        {
            if (_contents.Count == 0) { textWriter.WriteLine("EMPTY"); return; }

            foreach (KeyValuePair<T, int> entry in _contents)
            {
                textWriter.WriteLine($"'{entry.Key.Name}' x{entry.Value}");
            }
        }
    }
}
