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
        /// Copies the contents of the collection into a new array.
        /// </summary>
        /// 
        /// <typeparam name="T">The collection type.</typeparam>
        /// <param name="collection">The collection to copy.</param>
        /// 
        /// <returns>The copied array.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if the collection is null.</exception>
        public static T[] Copy<T>(this ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            T[] copy = new T[collection.Count];
            collection.CopyTo(copy, 0);
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
            T[] copy = new T[length];
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
            T[,] copy = new T[xLength, yLength];
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
        public static T[] TrimFromStart<T>(this T[] array) where T : class
        {
            if (array == null) throw new ArgumentNullException("array");

            int indexOfFirstNull;
            for (indexOfFirstNull = 0; indexOfFirstNull < array.Length; ++indexOfFirstNull)
            {
                if (array[indexOfFirstNull] == null) break;
            }

            //New array length = indexOfFirstNull
            T[] trimmedArray = new T[indexOfFirstNull];
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
        public static T[] TrimFromEnd<T>(this T[] array) where T : class
        {
            if (array == null) throw new ArgumentNullException("array");

            int indexOfFirstValue;
            for (indexOfFirstValue = array.Length - 1; indexOfFirstValue >= 0; --indexOfFirstValue)
            {
                if (array[indexOfFirstValue] != null) break;
            }

            int newLength = indexOfFirstValue + 1;

            T[] trimmedArray = new T[newLength];
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
            T[] copy = new T[length];
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