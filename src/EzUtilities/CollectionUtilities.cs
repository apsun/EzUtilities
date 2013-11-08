using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzUtilities
{
    /// <summary>
    /// Provides utilities for working with generic collections.
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Checks whether the IEnumerable contains multiple equal values.
        /// </summary>
        /// <param name="items">The IEnumerable to check.</param>
        /// <typeparam name="T">The type of items in the IEnumerable.</typeparam>
        /// <returns>True if an equal pair was found; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the IEnumerable or the comparer are null.</exception>
        public static bool ContainsDuplicates<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            var set = new HashSet<T>();
            return items.Any(item => !set.Add(item));
        }

        /// <summary>
        /// Checks whether the collection is null or empty.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <returns>True if the collection is null or empty; false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Gets the indices corresponding to the items in a list. 
        /// Returns -1 for the index if the item was not found.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="items">The items to find.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>The indices of the items within the list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list or the items are null.</exception>
        public static int[] IndexOf<T>(this IList<T> list, IList<T> items)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) throw new ArgumentNullException("items");
            var indices = new int[items.Count];
            for (int i = 0; i < items.Count; ++i)
            {
                int index = list.IndexOf(items[i]);
                indices[i] = index;
            }
            return indices;
        }

        /// <summary>
        /// Gets the indices corresponding to the items in a list. 
        /// Returns -1 for the index if the item was not found.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="items">The items to find.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>The indices of the items within the list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list or the items are null.</exception>
        public static IEnumerable<int> IndexOf<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) throw new ArgumentNullException("items");
            return items.Select(list.IndexOf);
        }

        /// <summary>
        /// Checks if the index is within range for this list.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="index">The index to check.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>True if the index is [0, length); false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        public static bool IsValidIndex<T>(this IList<T> list, int index)
        {
            if (list == null) throw new ArgumentNullException("list");
            return index >= 0 && index < list.Count;
        }

        /// <summary>
        /// Performs an insertion sort on a list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
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
        /// Rearranges multiple items within a list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to rearrange.</param>
        /// <param name="destIndex">The index to insert the rearranged items at.</param>
        /// <param name="items">The items to rearrange within the list.</param>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an item was not found in the list or if the list is read-only.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the destination index is out of range.</exception>
        /// <exception cref="ArgumentException">Thrown if there are duplicate values in the selected items.</exception>
        public static void Rearrange<T>(this IList<T> list, int destIndex, params T[] items)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (!list.IsValidRearrangeIndex(destIndex))
                throw new ArgumentOutOfRangeException("destIndex");

            if (items.IsNullOrEmpty()) return;

            if (items.Length == 1)
            {
                list.Rearrange(destIndex, items[0]);
                return;
            }

            int newdestIndex = destIndex;
            var selectedIndices = new int[items.Length];

            var set = new HashSet<T>();
            for (int i = 0; i < items.Length; ++i)
            {
                T item = items[i];

                if (!set.Add(item))
                    throw new ArgumentException("Selected items contains duplicates", "items");

                int indexInList = list.IndexOf(item);
                if (indexInList == -1)
                    throw new InvalidOperationException("One or more items were not found in the list");

                selectedIndices[i] = indexInList;
                if (indexInList < destIndex)
                {
                    --newdestIndex;
                }
            }

            //Sort for better performance on the two steps. 
            //First, it makes getting the maximum and minimum easier. 
            //Second, it allows branch prediction to speed up the foreach loop below.
            selectedIndices.Sort();

            //Get the minimum and maximum srcIndex indices of the rearranged items. 
            //This marks the region that will be modified.
            int minSourceIndex = selectedIndices[0];
            int maxSourceIndex = selectedIndices[selectedIndices.Length - 1];

            //Get the amount of selected items on the left and on the right. 
            //We use this to determine the amount of items that need to be shifted. 
            //For some reason, incrementing both numbers is a bit faster 
            //than incrementing one number and subtracting it from the length.
            int leftSelected = 0;
            int rightSelected = 0;
            foreach (int itemIndex in selectedIndices)
            {
                if (itemIndex < destIndex)
                {
                    ++leftSelected;
                }
                else
                {
                    ++rightSelected;
                }
            }

            //Get amount of items to shift. 
            //Equal to the number of non-selected items between the 
            //minimum index and the target index, and the number of 
            //non-selected items between the target index and the maximum index. 
            //These can be negative, just means that max < destIndex or min >= destIndex 
            //It doesn't affect the outcome, since it'll just fail the entry condition 
            //in the for loops below. Alternatively, just contrain them to zero.
            int leftShiftCount = destIndex - minSourceIndex - leftSelected;
            int rightShiftCount = maxSourceIndex - destIndex - rightSelected + 1;

            //Now we shift items to create a gap in the middle where the 
            //rearranged items go. We start from the ends of the list, 
            //since we will overwrite existing values. 
            //This part shifts items left of the target index further left...
            int shiftedItemIndex = minSourceIndex;
            for (int i = minSourceIndex; i < minSourceIndex + leftShiftCount; ++i)
            {
                do { } while (selectedIndices.Contains(++shiftedItemIndex));
                list[i] = list[shiftedItemIndex];
            }

            //...and this part shifts items right of the target index further right.
            shiftedItemIndex = maxSourceIndex;
            for (int i = maxSourceIndex; i > maxSourceIndex - rightShiftCount; --i)
            {
                do { } while (selectedIndices.Contains(--shiftedItemIndex));
                list[i] = list[shiftedItemIndex];
            }

            //Finally, insert the items into the list where they belong.
            for (int i = 0; i < items.Length; ++i)
            {
                list[newdestIndex + i] = items[i];
            }
        }

        /// <summary>
        /// Rearranges multiple items within a list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to rearrange.</param>
        /// <param name="destIndex">The index to insert the rearranged items at.</param>
        /// <param name="srcIndices">The indices of the items to rearrange within the list.</param>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an item was not found in the list or if the list is read-only.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the destination or source indices are out of range.</exception>
        /// <exception cref="ArgumentException">Thrown if there are duplicate values in the selected items.</exception>
        public static void RearrangeAt<T>(this IList<T> list, int destIndex, params int[] srcIndices)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (!list.IsValidRearrangeIndex(destIndex))
                throw new ArgumentOutOfRangeException("destIndex");

            if (srcIndices.IsNullOrEmpty()) return;

            if (srcIndices.Length == 1)
            {
                list.RearrangeAt(destIndex, srcIndices[0]);
                return;
            }

            int newdestIndex = destIndex;
            var items = new T[srcIndices.Length];

            var set = new HashSet<int>();
            for (int i = 0; i < srcIndices.Length; ++i)
            {
                int indexInList = srcIndices[i];

                if (!list.IsValidIndex(indexInList))
                    throw new ArgumentOutOfRangeException("srcIndices", "One or more source indices are out of range");

                if (!set.Add(indexInList))
                    throw new ArgumentException("Source indices contains duplicates", "srcIndices");

                T item = list[indexInList];
                items[i] = item;
                if (indexInList < destIndex)
                {
                    --newdestIndex;
                }
            }

            int[] sortedIndices = srcIndices.SortCopy();

            int minSourceIndex = sortedIndices[0];
            int maxSourceIndex = sortedIndices[sortedIndices.Length - 1];

            int leftSelected = 0;
            int rightSelected = 0;
            foreach (int itemIndex in sortedIndices)
            {
                if (itemIndex < destIndex)
                {
                    ++leftSelected;
                }
                else
                {
                    ++rightSelected;
                }
            }

            int leftShiftCount = destIndex - minSourceIndex - leftSelected;
            int rightShiftCount = maxSourceIndex - destIndex - rightSelected + 1;

            int shiftedItemIndex = minSourceIndex;
            for (int i = minSourceIndex; i < minSourceIndex + leftShiftCount; ++i)
            {
                do { } while (sortedIndices.Contains(++shiftedItemIndex));
                list[i] = list[shiftedItemIndex];
            }

            shiftedItemIndex = maxSourceIndex;
            for (int i = maxSourceIndex; i > maxSourceIndex - rightShiftCount; --i)
            {
                do { } while (sortedIndices.Contains(--shiftedItemIndex));
                list[i] = list[shiftedItemIndex];
            }

            for (int i = 0; i < items.Length; ++i)
            {
                list[newdestIndex + i] = items[i];
            }
        }

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="item">The item to rearrange.</param>
        /// <param name="destIndex">The index to insert the rearranged item at.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the item was not found in the list.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Target index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void Rearrange<T>(this IList<T> list, int destIndex, T item)
        {
            if (list == null) throw new ArgumentNullException("list");

            int srcIndex = list.IndexOf(item);
            if (srcIndex == -1)
                throw new InvalidOperationException("Item was not found in the list");

            if (destIndex == srcIndex) return;

            if (!list.IsValidRearrangeIndex(destIndex))
                throw new ArgumentOutOfRangeException("destIndex", "Target index out of range");

            if (destIndex > srcIndex)
            {
                --destIndex;
                for (int i = srcIndex; i < destIndex; ++i)
                {
                    list[i] = list[i + 1];
                }
            }
            else
            {
                for (int i = srcIndex; i > destIndex; --i)
                {
                    list[i] = list[i - 1];
                }
            }

            list[destIndex] = item;
        }

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="srcIndex">The index of the item to rearrange.</param>
        /// <param name="destIndex">The index to insert the rearranged item at.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Source or destination index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void RearrangeAt<T>(this IList<T> list, int destIndex, int srcIndex)
        {
            if (list == null) throw new ArgumentNullException("list");

            if (destIndex == srcIndex) return;

            if (!list.IsValidRearrangeIndex(destIndex))
                throw new ArgumentOutOfRangeException("destIndex", "Target index out of range");

            if (!list.IsValidIndex(srcIndex))
                throw new ArgumentOutOfRangeException("srcIndex", "Source index out of range");

            T item = list[srcIndex];

            if (destIndex > srcIndex)
            {
                --destIndex;
                for (int i = srcIndex; i < destIndex; ++i)
                {
                    list[i] = list[i + 1];
                }
            }
            else
            {
                for (int i = srcIndex; i > destIndex; --i)
                {
                    list[i] = list[i - 1];
                }
            }

            list[destIndex] = item;
        }

        /// <summary>
        /// Removes multiple items from a list. 
        /// Returns whether all items were successfully removed from the list.
        /// </summary>
        /// <param name="list">The list to remove items from.</param>
        /// <param name="items">The items to remove.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>Whether all items were removed from the list.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static bool Remove<T>(this IList<T> list, params T[] items)
        {
            if (items != null && items.Length == 1)
            {
                if (list != null) return list.Remove(items[0]);
            }

            return list.Remove((IEnumerable<T>)items);
        }

        /// <summary>
        /// Removes multiple items from a list. 
        /// Returns whether all items were successfully removed from the list.
        /// </summary>
        /// <param name="list">The list to remove items from.</param>
        /// <param name="items">The items to remove.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>Whether all items were removed from the list.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static bool Remove<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) return true;

            return items.Aggregate(true, (current, item) => list.Remove(item) && current);
        }

        /// <summary>
        /// Removes multiple items from a list.
        /// </summary>
        /// <param name="list">The list to remove items from.</param>
        /// <param name="indices">The indices of the items to remove.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="ArgumentException">Thrown if indices contains duplicates.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void RemoveAt<T>(this IList<T> list, params int[] indices)
        {
            if (list == null) throw new ArgumentNullException("list");

            if (indices.IsNullOrEmpty()) return;

            if (indices.Length == 1)
            {
                list.RemoveAt(indices[0]);
                return;
            }

            int[] reverseInd = indices.ReverseSortCopy();

            if (indices[0] >= list.Count || indices[indices.Length - 1] < 0)
            {
                throw new ArgumentOutOfRangeException("indices", "One or more indices are out of range");
            }

            for (int i = 0; i < reverseInd.Length - 1; ++i)
            {
                if (reverseInd[i] == reverseInd[i + 1])
                {
                    throw new ArgumentException("Indices contains duplicates", "indices");
                }
            }

            foreach (int index in reverseInd)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes the first item in the list that satisfies a condition. 
        /// Returns whether an item was removed.
        /// </summary>
        /// <param name="list">The list to remove the item from.</param>
        /// <param name="predicate">The predicate that determines whether to remove an item.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>Whether an item was removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list or the predicate is null.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static bool RemoveFirst<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (predicate == null) throw new ArgumentNullException("predicate");

            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                if (!predicate(item)) continue;
                list.RemoveAt(i);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all items in the list that satisfy a condition. 
        /// Returns whether any items were removed.
        /// </summary>
        /// <param name="list">The list to remove the items from.</param>
        /// <param name="predicate">The predicate that determines whether to remove an item.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>Whether any items were removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list or the predicate is null.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static bool RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (predicate == null) throw new ArgumentNullException("predicate");

            bool anyRemoved = false;
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                if (!predicate(item)) continue;
                list.RemoveAt(i);
                anyRemoved = true;
                --i;
            }
            return anyRemoved;
        }

        /// <summary>
        /// Pseudo-randomly shuffles a list using the Knuth-Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            int n = list.Count;
            while (n-- > 0)
            {
                int k = RandomUtilities.Integer(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Checks if the index is a valid index for this list 
        /// or is equal to the list's count (for rearranging purposes).
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="index">The index to check.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>True if the index is [0, length]; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        private static bool IsValidRearrangeIndex<T>(this IList<T> list, int index)
        {
            if (list == null) throw new ArgumentNullException("list");
            return index >= 0 && index <= list.Count;
        }

        /// <summary>
        /// Returns a string representation of the collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="items">The collection.</param>
        /// <returns>The string containing all elements of the collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if items is null.</exception>
        public static string AsString<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            var sb = new StringBuilder('{');
            bool hasItem = false;
            foreach (T item in items)
            {
                if (hasItem)
                    sb.Append(',');
                else
                    hasItem = true;
                sb.Append(' ');
                sb.Append(item);
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}