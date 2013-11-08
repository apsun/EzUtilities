using System;
using System.Diagnostics.Contracts;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to calculate numerical values.
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Finds the minimum value of a set.
        /// </summary>
        /// <param name="values">The set of values.</param>
        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            Contract.Requires<ArgumentNullException>(values != null);
            Contract.Requires<ArgumentException>(values.Length > 0);
            return FindExtreme(values, cmp => cmp < 0);
        }

        /// <summary>
        /// Finds the maximum value of a set.
        /// </summary>
        /// <param name="values">The set of values.</param>
        public static T Max<T>(params T[] values) where T : IComparable<T>
        {
            Contract.Requires<ArgumentNullException>(values != null);
            Contract.Requires<ArgumentException>(values.Length > 0);
            return FindExtreme(values, cmp => cmp > 0);
        }

        /// <summary>
        /// Determines whether the value is within a range of [lower, upper].
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The inclusive upper bound.</param>
        public static bool IsBetween<T>(this T value, T lower, T upper) where T : IComparable<T>
        {
            Contract.Requires<ArgumentOutOfRangeException>(upper.CompareTo(lower) >= 0);
            return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
        }

        /// <summary>
        /// Wraps a number within a range of [lower, upper).
        /// </summary>
        /// <param name="num">The number to wrap.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The exclusive upper bound.</param>
        public static int Wrap(this int num, int lower, int upper)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upper > lower);
            int t = (num - lower) % (upper - lower);
            return t < 0 ? t + upper : t + lower;
        }

        /// <summary>
        /// Wraps a number within a range of [lower, upper).
        /// </summary>
        /// <param name="num">The number to wrap.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The exclusive upper bound.</param>
        public static float Wrap(this float num, float lower, float upper)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upper > lower);
            float t = (num - lower) % (upper - lower);
            return t < 0 ? t + upper : t + lower;
        }

        /// <summary>
        /// Wraps a number within a range of [lower, upper).
        /// </summary>
        /// <param name="num">The number to wrap.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The exclusive upper bound.</param>
        public static double Wrap(this double num, double lower, double upper)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upper > lower);
            double t = (num - lower) % (upper - lower);
            return t < 0 ? t + upper : t + lower;
        }

        /// <summary>
        /// Clamps a number within a range of [lower, upper].
        /// </summary>
        /// <param name="value">The number to clamp.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The inclusive upper bound.</param>
        public static T Clamp<T>(this T value, T lower, T upper) where T : IComparable<T>
        {
            Contract.Requires<ArgumentOutOfRangeException>(upper.CompareTo(lower) >= 0);
            if (value.CompareTo(lower) <= 0) return lower;
            if (value.CompareTo(upper) >= 0) return upper;
            return value;
        }

        private static T FindExtreme<T>(T[] values, Func<int, bool> cmp) where T : IComparable<T>
        {
            T extreme = values[0];
            for (int i = 1; i < values.Length; ++i)
            {
                T value = values[i];
                if (cmp(value.CompareTo(extreme))) extreme = value;
            }
            return extreme;
        }
    }
}
