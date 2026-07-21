using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PraktikaNORBIT_Collections
{
    public class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        public SmartStack()
        {
            _items = new T[4];
            _count = 0;
        }

        public SmartStack(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Ёмкость должна быть положительной.");
            _items = new T[capacity];
            _count = 0;
        }

        public SmartStack(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                _items = Array.Empty<T>();
                _count = 0;
            }
            else
            {
                _items = collection.ToArray();
                _count = _items.Length;
                Array.Reverse(_items);
            }
        }

        public void Push(T item)
        {
            if (_count == _items.Length)
            {
                T[] newArray = new T[_count * 2];
                Array.Copy(_items, newArray, _count);
                _items = newArray;
            }
            _items[_count] = item;
            _count++;
        }

        public void PushRange(IEnumerable<T> collection)
        {
            if (collection == null) return;

            int addCount = collection.Count();
            if (_count + addCount > _items.Length)
            {
                int newCapacity = _items.Length;
                while (newCapacity < _count + collection.Count())
                {
                    newCapacity *= 2;
                }
                T[] newArray = new T[newCapacity];
                Array.Copy(_items, newArray, _count);
                _items = newArray;
            }

            int index = _count + addCount - 1;
            foreach (T item in collection.Reverse())
            {
                _items[index--] = item;
            }
            _count += addCount;
        }

        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст.");

            T item = _items[_count - 1];
            _items[_count - 1] = default(T);
            _count--;
            return item;

        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст.");
            return _items[_count - 1];
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(item, _items[i]))
                    return true;
            }
            return false;
        }

        public int Count
        {
            get { return _count; }
        }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("Выход за границы стека.");
                return _items[_count - 1 - index];
            }
        }
    }
}
