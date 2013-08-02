using System;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to work with arrays.
    /// </summary>
    public static class ArrayUtilities
    {
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
    }
}