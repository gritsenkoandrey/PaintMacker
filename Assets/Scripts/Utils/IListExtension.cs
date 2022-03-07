using System;
using System.Collections.Generic;

namespace Utils
{
    public static class ListExtension
    {
        public static void ForEach<T>(this IList<T> source, Action<T> action)
        {
            for (int i = 0; i < source.Count; i++)
            {
                action(source[i]);
            }
        }
        
        public static void For<T>(this IList<T> source, Action<T, int> action)
        {
            for (int i = 0; i < source.Count; i++)
            {
                action(source[i], i);
            }
        }
        
        public static T GetFirst<T>(this IList<T> source)
        {
            return source[0];
        }

        public static T GetLast<T>(this IList<T> source)
        {
            return source[source.Count - 1];
        }

        public static T GetFirstAndRemove<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            T t = list[0];

            list.RemoveAt(0);

            return t;
        }

        public static T GetLastAndRemove<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            T t = list[list.Count - 1];

            list.RemoveAt(list.Count - 1);

            return t;
        }

        public static T GetRandom<T>(this IList<T> source) 
        {
            int index = U.Random(0, source.Count);
            
            return source[index];
        }

        public static T GetRandomAndRemove<T>(this IList<T> source)
        {
            if (source.Count == 0)
            {
                return default(T);
            }
        
            int index = U.Random(0, source.Count);
            
            T outer = source[index];
        
            source.RemoveAt(index);
        
            return outer;
        }
    }
}