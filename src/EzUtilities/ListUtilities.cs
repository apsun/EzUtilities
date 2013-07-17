using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EzUtilities
{
    /// <summary>
    /// Provides utilities for working with generic lists.
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// Checks whether the list contains multiple equal values using the default comparer.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>True if an equal pair was found; false otherwise.</returns>
        public static bool ContainsDuplicates<T>(this IList<T> list)
        {
            return list.ContainsDuplicates(Comparer<T>.Default);
        }

        /// <summary>
        /// Checks whether the list contains multiple equal values using the specified comparer.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="comparer">The comparer used to check for equality.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>True if an equal pair was found; false otherwise.</returns>
        public static bool ContainsDuplicates<T>(this IList<T> list, IComparer<T> comparer)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                for (int j = i + 1; j < list.Count; ++j)
                {
                    if (comparer.Compare(list[i], list[j]) == 0) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the index is within range for this list.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="index">The index to check.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <returns>True if the index is between 0 and the list's length; false otherwise.</returns>
        public static bool IsValidIndex<T>(this IList<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

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
        /// Rearranges multiple items within a list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to rearrange.</param>
        /// <param name="targetIndex">The index to insert the rearranged items.</param>
        /// <param name="items">The items to rearrange within the list.</param>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an item was not found in the list or if the list is read-only.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target index is out of range.</exception>
        /// <exception cref="ArgumentException">Thrown if there are duplicate values in the selected items.</exception>
        public static void Rearrange<T>(this IList<T> list, int targetIndex, params T[] items) where T : class
        {
            if (list == null) throw new ArgumentNullException("list");
            if (targetIndex < 0 || targetIndex > list.Count) throw new ArgumentOutOfRangeException("targetIndex");
            if (items.IsNullOrEmpty()) return;

            int newTargetIndex = targetIndex;
            int[] selectedIndicies = new int[items.Length];

            for (int i = 0; i < items.Length; ++i)
            {
                T item = items[i];

                for (int j = i + 1; j < items.Length; ++j)
                {
                    //TODO: Maybe allow selection of multiple items, 
                    //TODO: Like selecting {1,2,1} from a list of {1,1,2,3,2,4}
                    if (item == items[j]) throw new ArgumentException("Items contains duplicates");
                }

                int indexInList = list.IndexOf(item);
                if (indexInList == -1) throw new InvalidOperationException();
                selectedIndicies[i] = indexInList;
                if (indexInList < targetIndex)
                {
                    --newTargetIndex;
                }
            }

            //Sort for better performance on the two steps. 
            //First, it makes getting the maximum and minimum easier. 
            //Second, it allows branch prediction to speed up the foreach loop below.
            selectedIndicies.Sort();

            //Get the minimum and maximum original indicies of the rearranged items. 
            //This marks the region that will be modified.
            int minOriginalIndex = selectedIndicies[0];
            int maxOriginalIndex = selectedIndicies[selectedIndicies.Length - 1];

            //Get the amount of selected items on the left and on the right. 
            //We use this to determine the amount of items that need to be shifted. 
            //For some reason, incrementing both numbers is a bit faster 
            //than incrementing one number and subtracting it from the length.
            int leftSelected = 0;
            int rightSelected = 0;
            foreach (int itemIndex in selectedIndicies)
            {
                if (itemIndex < targetIndex)
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
            //These can be negative, just means that max < targetIndex or min >= targetIndex 
            //It doesn't affect the outcome, since it'll just fail the entry condition 
            //in the for loops below. Alternatively, just contrain them to zero.
            int leftShiftCount = targetIndex - minOriginalIndex - leftSelected;
            int rightShiftCount = maxOriginalIndex - targetIndex - rightSelected + 1;
            
            //Now we shift items to create a gap in the middle where the 
            //rearranged items go. We start from the ends of the list, 
            //since we will overwrite existing values. 
            //This part shifts items left of the target index further left...
            int shiftedItemIndex = minOriginalIndex;
            for (int i = minOriginalIndex; i < minOriginalIndex + leftShiftCount; ++i)
            {
                do { } while (selectedIndicies.Contains(++shiftedItemIndex));
                list[i] = list[shiftedItemIndex];
            }

            //...and this part shifts items right of the target index further right.
            shiftedItemIndex = maxOriginalIndex;
            for (int i = maxOriginalIndex; i > maxOriginalIndex - rightShiftCount; --i)
            {
                do { } while (selectedIndicies.Contains(--shiftedItemIndex));
                list[i] = list[shiftedItemIndex];
            }

            //Finally, insert the items into the list where they belong.
            for (int i = 0; i < items.Length; ++i)
            {
                list[newTargetIndex + i] = items[i];
            }
        }

        /// <summary>
        /// Moves an item within a list to a new index.
        /// </summary>
        /// <param name="list">The list that contains the item.</param>
        /// <param name="item">The item to move.</param>
        /// <param name="targetIndex">The index at which to insert the item at.</param>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the item was not found in the list.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Target index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public static void Rearrange<T>(this IList<T> list, int targetIndex, T item)
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
        /// <typeparam name="T">The type of items in the list.</typeparam>
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
        /// Pseudo-randomly shuffles a list using the Knuth-Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n-- > 0)
            {
                int k = RandomUtilities.GetRandomInteger(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

namespace EzUtilities.NonGeneric
{
    /// <summary>
    /// Provides utilities for working with non-generic lists.
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// Checks whether the list contains multiple equal values using the default comparer.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <returns>True if an equal pair was found; false otherwise.</returns>
        public static bool ContainsDuplicates(this IList list)
        {
            return list.ContainsDuplicates(Comparer.Default);
        }

        /// <summary>
        /// Checks whether the list contains multiple equal values using the specified comparer.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="comparer">The comparer used to check for equality.</param>
        /// <returns>True if an equal pair was found; false otherwise.</returns>
        public static bool ContainsDuplicates(this IList list, IComparer comparer)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                for (int j = i + 1; j < list.Count; ++j)
                {
                    if (comparer.Compare(list[i], list[j]) == 0) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the index is within range for this list.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="index">The index to check.</param>
        /// <returns>True if the index is between 0 and the list's length; false otherwise.</returns>
        public static bool IsValidIndex(this IList list, int index)
        {
            return index >= 0 && index < list.Count;
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

        /// <summary>
        /// Pseudo-randomly shuffles a list using the Knuth-Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle(this IList list)
        {
            int n = list.Count;
            while (n-- > 0)
            {
                int k = RandomUtilities.GetRandomInteger(n + 1);
                object value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}