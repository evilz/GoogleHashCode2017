using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pizza.Utils
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> actionWithIndex)
        {
            var index = 0;
            foreach (var item in source)
            {
                actionWithIndex(item, index);
                index += 1;
            }
        }
    }

    public static class ParallelismHelper
    {
        public static Action<IEnumerable<T>, Action<T>> ForEach<T>(bool parallel)
        {
            if (parallel)
            {
                return (source, action) => Parallel.ForEach(source, action);
            }
            return (source, action) => source.ForEach(action);
        }
    }
}