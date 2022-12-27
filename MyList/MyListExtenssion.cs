using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyList
{
    public static class MyListExtenssion
    {
        public static TSource Single<TSource>(this MyList<TSource> source)
        {
            if (source.Count != 1)
                throw new MoreThanOneMatchingElementException("Source contains more than one element");

            return source[0];
        }

        public static TSource Single<TSource>(this MyList<TSource> source, Func<TSource, bool> predicate)
        {
            int count = 0;
            TSource entityToReturn = default;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    count++;
                    if (count > 1)
                        throw new Exception("Source contains more than one element");

                    entityToReturn = item;
                }
            }

            if (count == 0)
                throw new Exception("Sequence contains no matching element");

            return entityToReturn;
        }

        public static TSource SingleOrDefault<TSource>(this MyList<TSource> source)
        {
            if (source.Count > 1)
                throw new Exception("Source contains more than one element");

            if (source.Count == 0)
                return default;

            return source[0];
        }

        public static TSource SingleOrDefault<TSource>(this MyList<TSource> source, Func<TSource, bool> predicate)
        {
            int count = 0;
            TSource entityToReturn = default;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    count++;
                    entityToReturn = item;
                }
            }

            if (count > 1)
                throw new Exception("Source contains more than one element");

            return entityToReturn;
        }

        public static IEnumerable<TSource> Where<TSource>(this MyList<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        public static IEnumerable<IGrouping<TKey, TSource>> MyGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            Dictionary<TKey, List<TSource>> groups = new Dictionary<TKey, List<TSource>>();

            foreach (TSource element in source)
            {
                TKey key = keySelector(element);

                if (groups.ContainsKey(key))
                {
                    groups[key].Add(element);
                }
                else
                {
                    groups[key] = new List<TSource> { element };
                }
            }

            return groups.Select(g => (IGrouping<TKey, TSource>)new Grouping<TKey, TSource>(g.Key, g.Value));
        }
    }

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; }
        public IEnumerable<TElement> Elements { get; }

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            Key = key;
            Elements = elements;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var item in Elements)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}