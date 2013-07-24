using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace EzUtilities
{
    /// <summary>
    /// Provides utilities for working with generic collections.
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Checks whether the collection is null or empty.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <returns>True if the collection is null or empty; false otherwise.</returns>
        [Pure]
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

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
        [Pure]
        public static T[] Copy<T>(this ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            T[] copy = new T[collection.Count];
            collection.CopyTo(copy, 0);
            return copy;
        }
    }
}