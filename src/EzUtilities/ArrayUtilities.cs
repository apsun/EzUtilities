using System;
using System.Collections.Generic;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to work with arrays.
    /// </summary>
    public static class ArrayUtilities
    {
        /// <summary>
        /// Returns the index of the first occurrence of an item within the array.
        /// </summary>
        /// <param name="array">The array to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <returns>The index of the item if it was found; -1 otherwise.</returns>
        public static int IndexOf<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item);
        }

        /// <summary>
        /// Gets the indices corresponding to the items in an array. 
        /// Returns -1 for the index if the item was not found.
        /// </summary>
        /// <param name="array">The array to search.</param>
        /// <param name="items">The items to find.</param>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <returns>The indices of the items within the array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the array or the items are null.</exception>
        public static int[] IndexOf<T>(this T[] array, IList<T> items)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (items == null) throw new ArgumentNullException("items");

            var indices = new int[items.Count];
            for (int i = 0; i < indices.Length; ++i)
            {
                indices[i] = Array.IndexOf(array, items[i]);
            }
            return indices;
        }

        /// <summary>
        /// Determines whether an item is in the array.
        /// </summary>
        /// <param name="array">The array to search.</param>
        /// <param name="item">The item to search for.</param>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <returns>True if the item was found in the array; false otherwise.</returns>
        public static bool Contains<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item) >= 0;
        }

        /// <summary>
        /// Performs an insertion sort on an array.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        public static void InsertionSort<T>(this T[] array) where T : IComparable<T>
        {
            if (array == null) throw new ArgumentNullException("array");

            for (int i = 1; i < array.Length; ++i)
            {
                T item = array[i];
                int j = i;
                while (j > 0 && (array[j - 1].CompareTo(item) != 1))
                {
                    array[j] = array[j - 1];
                    --j;
                }
                array[j] = item;
            }
        }

        /// <summary>
        /// Sorts the array in-place.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// <typeparam name="T">The type of the values in the array.</typeparam>
        /// <exception cref="System.ArgumentNullException">Thrown if the array is null.</exception>
        public static void Sort<T>(this T[] array) where T : IComparable<T>
        {
            Array.Sort(array);
        }

        /// <summary>
        /// Sorts an array in descending order.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// <typeparam name="T">The type of the values in the array.</typeparam>
        /// <exception cref="System.ArgumentNullException">Thrown if the array is null.</exception>
        /// <exception cref="System.ArgumentException">The implementation of comparison caused an error during the sort. For example, comparison might not return 0 when comparing an item with itself.</exception>
        public static void ReverseSort<T>(this T[] array) where T : IComparable<T>
        {
            Array.Sort(array, (a, b) => b.CompareTo(a));
        }

        /// <summary>
        /// Creates a sorted copy of the array.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// <typeparam name="T">The type of the values in the array.</typeparam>
        /// <exception cref="System.ArgumentNullException">Thrown if the array is null.</exception>
        public static T[] SortCopy<T>(this T[] array) where T : IComparable<T>
        {
            T[] copy = array.Copy();
            Array.Sort(copy);
            return copy;
        }

        /// <summary>
        /// Creates a copy of the array sorted in descending order.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// <typeparam name="T">The type of the values in the array.</typeparam>
        /// <exception cref="System.ArgumentNullException">Thrown if the array is null.</exception>
        /// <exception cref="System.ArgumentException">The implementation of comparison caused an error during the sort. For example, comparison might not return 0 when comparing an item with itself.</exception>
        public static T[] ReverseSortCopy<T>(this T[] array) where T : IComparable<T>
        {
            T[] copy = array.Copy();
            Array.Sort(copy, (a, b) => b.CompareTo(a));
            return copy;
        }

        /// <summary>
        /// Copies the contents of the array into a new array.
        /// </summary>
        /// 
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// 
        /// <returns>The copied array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        public static T[] Copy<T>(this T[] array)
        {
            if (array == null) throw new ArgumentNullException("array");
            int length = array.Length;
            var copy = new T[length];
            Array.Copy(array, copy, length);
            return copy;
        }

        /// <summary>
        /// Copies the contents of the 2D array into a new array.
        /// </summary>
        /// 
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// 
        /// <returns>The copied array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        public static T[,] Copy<T>(this T[,] array)
        {
            if (array == null) throw new ArgumentNullException("array");
            int xLength = array.GetLength(0);
            int yLength = array.GetLength(1);
            var copy = new T[xLength, yLength];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        /// <summary>
        /// Copies the contents of the array into a new array.
        /// </summary>
        /// <param name="array">The array to copy.</param>
        /// 
        /// <returns>The copied array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        public static Array Copy(this Array array)
        {
            if (array == null) throw new ArgumentNullException("array");
            return (Array)array.Clone();
        }

        /// <summary>
        /// Resizes an array to remove all elements after and including the first null element.
        /// </summary>
        /// 
        /// <param name="array">The array to resize.</param>
        /// 
        /// <returns>The resized array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if array is null.</exception>
        public static T[] TrimNullsFromStart<T>(this T[] array) where T : class
        {
            if (array == null) throw new ArgumentNullException("array");

            int indexOfFirstNull;
            for (indexOfFirstNull = 0; indexOfFirstNull < array.Length; ++indexOfFirstNull)
            {
                if (array[indexOfFirstNull] == null) break;
            }

            var trimmedArray = new T[indexOfFirstNull];
            Array.Copy(array, trimmedArray, indexOfFirstNull);
            return trimmedArray;
        }

        /// <summary>
        /// Resizes an array to remove all trailing nulls from the end of the array.
        /// </summary>
        /// 
        /// <param name="array">The array to resize.</param>
        /// 
        /// <returns>The resized array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if array is null.</exception>
        public static T[] TrimNullsFromEnd<T>(this T[] array) where T : class
        {
            if (array == null) throw new ArgumentNullException("array");

            int indexOfFirstValue;
            for (indexOfFirstValue = array.Length - 1; indexOfFirstValue >= 0; --indexOfFirstValue)
            {
                if (array[indexOfFirstValue] != null) break;
            }

            int newLength = indexOfFirstValue + 1;

            var trimmedArray = new T[newLength];
            Array.Copy(array, trimmedArray, newLength);
            return trimmedArray;
        }

        /// <summary>
        /// Removes all null values from an array.
        /// </summary>
        /// 
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array to remove null values from.</param>
        /// 
        /// <returns>The array with null values removed.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if array is null.</exception>
        public static T[] RemoveNulls<T>(this T[] array) where T : class
        {
            if (array == null) throw new ArgumentNullException("array");

            int nullsRemoved = 0;
            int length = array.Length;
            var copy = new T[length];
            for (int i = 0; i < array.Length; ++i)
            {
                T current = array[i];
                if (current == null)
                {
                    ++nullsRemoved;
                }
                else
                {
                    copy[i - nullsRemoved] = current;
                }
            }
            if (nullsRemoved != 0)
            {
                Array.Resize(ref copy, length - nullsRemoved);
            }

            return copy;
        }
        
        /// <summary>
        /// Combines two arrays.
        /// </summary>
        /// 
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="first">The first array to copy values from. 
        /// The values of this array are placed first in the combined array.</param>
        /// <param name="second">The first array to copy values from.</param>
        /// 
        /// <returns>The combined array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if either array is null.</exception>
        public static T[] Combine<T>(T[] first, T[] second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var combined = new Level[first.Length + second.Length];
            Array.Copy(first, 0, combined, 0, first.Length);
            Array.Copy(second, 0, combined, first.Length, second.Length);
            return combined;
        }
    }
}