using System;
using System.Collections;
using System.Collections.Generic;

namespace MyList
{
    public class MyList<T> : IEnumerable<T>
    {
        #region Private Fields and Constructors

        private T[] _list;
        private int _count;
        private readonly IComparer<T> _comparear = Comparer<T>.Default;

        public MyList()
        {
            _list = new T[1];
            _count = 0;
            _comparear = Comparer<T>.Default;
        }

        public MyList(IComparer<T> comparer)
        {
            _list = new T[1];
            _count = 0;
            _comparear = comparer;
        }

        #endregion Private Fields and Constructors

        public int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                _count = value;
            }
        }

        #region Add Methods

        public void Add(T entity)
        {
            if (_list.Length == _count)
            {
                Resize();
            }

            _list[_count] = entity;
            _count++;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            if (entities is ICollection<T>)
            {
                foreach (var item in entities)
                {
                    Add(item);
                }
            }
            else
            {
                using (IEnumerator<T> en = entities.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Add(en.Current);
                    }
                }
            }
        }

        #endregion Add Methods

        #region Remove Methods

        public void Remove(T entity)
        {
            bool IsRemoved = false;

            for (int i = 0; i < _count; i++)
            {
                if (_comparear.Compare(_list[i], entity) == 0)
                {
                    var newList = new T[_count - 1];

                    for (int j = 0; j < _count; j++)
                    {
                        if (!IsRemoved)
                        {
                            if (j == i)
                            {
                                newList[j] = _list[j + 1];
                                IsRemoved = true;
                                _count--;
                                continue;
                            }

                            newList[j] = _list[j];
                            continue;
                        }
                        newList[j] = _list[j + 1];
                    }

                    _list = newList;
                    return;
                }
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            if (entities is ICollection<T>)
            {
                foreach (var item in entities)
                {
                    Remove(item);
                }
            }
            else
            {
                using (IEnumerator<T> en = entities.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Remove(en.Current);
                    }
                }
            }
        }

        public void RemoveRange(params T[] entities)
        {
            foreach (var item in entities)
            {
                Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException();

            var newList = new T[_count - 1];

            if (index == _count - 1)
            {
                Array.Copy(_list, 0, newList, 0, _count - 1);
                _list = newList;
                _count--;
                return;
            }
            bool IsRemoved = false;
            for (int i = 0; i < _count; i++)
            {
                if (!IsRemoved)
                {
                    if (i != index)
                    {
                        newList[i] = _list[i];
                        continue;
                    }

                    newList[i] = _list[i + 1];
                    _count--;
                    IsRemoved = true;
                }
                else
                {
                    newList[i] = _list[i + 1];
                }
            }
            _list = newList;
        }

        #endregion Remove Methods

        private void Resize()
        {
            var newList = new T[_list.Length * 2];
            Array.Copy(_list, newList, _list.Length);

            _list = newList;
        }

        public bool Contains(T entity)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_comparear.Compare(_list[i], entity) == 0)
                    return true;
            }

            return false;
        }

        public void Sort()
        {
            bool swaped;
            do
            {
                swaped = false;
                for (int i = 0; i < Count - 1; i++)
                {
                    if (_comparear.Compare(_list[i], _list[i + 1]) > 0)
                    {
                        (_list[i + 1], _list[i]) = (_list[i], _list[i + 1]);
                        swaped = true;
                    }
                }
            } while (swaped);
        }

        public T this[int index]
        {
            get
            {
                if (index > _count)
                    throw new ArgumentOutOfRangeException();
                return _list[index];
            }
            set
            {
                if (index > _count)
                    throw new ArgumentOutOfRangeException();
                _list[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _list[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}