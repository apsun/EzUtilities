using System;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to work with arrays.
    /// </summary>
    public static class ArrayUtilities
    {
        /// <summary>
        /// Copies the contents of the array into a new array.
        /// </summary>
        /// 
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// 
        /// <returns>The copied array.</returns>
        public static T[] Copy<T>(this T[] array)
        {
            int length = array.Length;
            T[] copy = new T[length];
            Array.Copy(array, copy, length);
            return copy;
        }

        /// <summary>
        /// Resizes an array to remove all elements after and including the first null element.
        /// </summary>
        /// 
        /// <param name="array">The array to resize.</param>
        /// 
        /// <returns>The resized array.</returns>
        public static T[] TrimFromStart<T>(this T[] array) where T : class
        {
            int endIndex = array.Length - 1;
            int indexOfFirstNull;
            for (indexOfFirstNull = 0; indexOfFirstNull <= endIndex; ++indexOfFirstNull)
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
        public static T[] TrimFromEnd<T>(this T[] array) where T : class
        {
            int startIndex = array.Length - 1;
            int indexOfFirstValue;
            for (indexOfFirstValue = startIndex; indexOfFirstValue >= 0; --indexOfFirstValue)
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
        public static T[] RemoveNulls<T>(this T[] array) where T : class
        {
            int nullsRemoved = 0;
            int length = array.Length;
            T[] copy = new T[length];
            for (int i = 0; i < length; ++i)
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