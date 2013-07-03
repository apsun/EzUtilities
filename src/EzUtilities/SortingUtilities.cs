using System;
using System.Collections.Generic;

namespace EzUtilities
{
    /// <summary>
    /// Provides utilities for sorting arrays and lists.
    /// </summary>
    public static class SortingUtilities
    {
        /// <summary>
        /// Performs an insertion sort on a list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        public static void InsertionSort<T>(this IList<T> list) where T : IComparable<T>
        {
            for (int i = 1; i < list.Count; ++i)
            {
                T item = list[i];
                int j = i;
                while (j > 0 && (list[j - 1].CompareTo(item) != 1))
                {
                    list[j] = list[j - 1];
                    --j;
                }
                list[j] = item;
            }
        }
    }
}