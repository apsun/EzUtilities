using System;
using System.Collections;
using System.Collections.Generic;

namespace EzUtilities
{
    /// <summary>
    /// Provides utilities for sorting and rearranging arrays and lists.
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// Performs an insertion sort on a list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        public static void InsertionSort<T>(this IList<T> list) where T : IComparable<T>
        {
            if (list == null) throw new ArgumentNullException("list");

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

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="item">The item to move.</param>
        /// <param name="targetIndex">The index at which to insert the item at.</param>
        /// <typeparam name="T">The type of the item and list.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the item was not found in the list.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Target index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void Rearrange<T>(this IList<T> list, T item, int targetIndex)
        {
            if (list == null) throw new ArgumentNullException("list");

            int originalIndex = list.IndexOf(item);
            if (originalIndex == -1) throw new InvalidOperationException();
            if (targetIndex == originalIndex) return;

            if (targetIndex > originalIndex)
            {
                for (int i = originalIndex; i < targetIndex; ++i)
                {
                    list[i] = list[i + 1];
                }
            }
            else
            {
                for (int i = originalIndex; i > targetIndex; --i)
                {
                    list[i] = list[i - 1];
                }
            }

            list[targetIndex] = item;
        }

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="originalIndex">The index of the item to move.</param>
        /// <param name="targetIndex">The index at which to insert the item at.</param>
        /// <typeparam name="T">The type of the item and list.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Source index does not exist in the list.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Target index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void RearrangeIndex<T>(this IList<T> list, int originalIndex, int targetIndex)
        {
            if (list == null) throw new ArgumentNullException("list");

            if (targetIndex == originalIndex) return;
            T item = list[originalIndex];

            if (targetIndex > originalIndex)
            {
                for (int i = originalIndex; i < targetIndex; ++i)
                {
                    list[i] = list[i + 1];
                }
            }
            else
            {
                for (int i = originalIndex; i > targetIndex; --i)
                {
                    list[i] = list[i - 1];
                }
            }

            list[targetIndex] = item;
        }

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="item">The item to move.</param>
        /// <param name="targetIndex">The index at which to insert the item at.</param>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the item was not found in the list.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Target index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void Rearrange(this IList list, object item, int targetIndex)
        {
            if (list == null) throw new ArgumentNullException("list");

            int originalIndex = list.IndexOf(item);
            if (originalIndex == -1) throw new InvalidOperationException();
            if (targetIndex == originalIndex) return;

            if (targetIndex > originalIndex)
            {
                for (int i = originalIndex; i < targetIndex; ++i)
                {
                    list[i] = list[i + 1];
                }
            }
            else
            {
                for (int i = originalIndex; i > targetIndex; --i)
                {
                    list[i] = list[i - 1];
                }
            }

            list[targetIndex] = item;
        }

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="originalIndex">The index of the item to move.</param>
        /// <param name="targetIndex">The index at which to insert the item at.</param>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Source index does not exist in the list.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Target index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void RearrangeIndex(this IList list, int originalIndex, int targetIndex)
        {
            if (list == null) throw new ArgumentNullException("list");

            if (targetIndex == originalIndex) return;
            object item = list[originalIndex];

            if (targetIndex > originalIndex)
            {
                for (int i = originalIndex; i < targetIndex; ++i)
                {
                    list[i] = list[i + 1];
                }
            }
            else
            {
                for (int i = originalIndex; i > targetIndex; --i)
                {
                    list[i] = list[i - 1];
                }
            }

            list[targetIndex] = item;
        }
    }
}