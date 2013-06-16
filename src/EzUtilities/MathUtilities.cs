using System;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for comparing numbers.
    /// </summary>
    public static class MathUtilities
    {
        private const float FloatEqualityEpsilon = 0.00001f;
        private const double DoubleEqualityEpsilon = 0.0000000001d;

        /// <summary>
        /// Checks whether two floats are approximately equal to each other.
        /// </summary>
        /// 
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <param name="epsilon">The tolerance for differences in values.</param>
        /// 
        /// <returns>Whether the two floats are approximately equal.</returns>
        public static bool ApproxEquals(this float a, float b, float epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// Checks whether two floats are approximately equal to each other 
        /// using the default tolerance epsilon (1E-05).
        /// </summary>
        /// 
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// 
        /// <returns>Whether the two floats are approximately equal.</returns>
        public static bool ApproxEquals(this float a, float b)
        {
            return ApproxEquals(a, b, FloatEqualityEpsilon);
        }

        /// <summary>
        /// Checks whether two doubles are approximately equal to each other.
        /// </summary>
        /// 
        /// <param name="a">The first double.</param>
        /// <param name="b">The second double.</param>
        /// <param name="epsilon">The tolerance for differences in values.</param>
        /// 
        /// <returns>Whether the two doubles are approximately equal.</returns>
        public static bool ApproxEquals(this double a, double b, double epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// Checks whether two doubles are approximately equal to each other 
        /// using the default tolerance epsilon (1E-10).
        /// </summary>
        /// 
        /// <param name="a">The first double.</param>
        /// <param name="b">The second double.</param>
        /// 
        /// <returns>Whether the two doubles are approximately equal.</returns>
        public static bool ApproxEquals(this double a, double b)
        {
            return ApproxEquals(a, b, DoubleEqualityEpsilon);
        }

        /// <summary>
        /// Finds the minimum value in a set of values.
        /// </summary>
        /// 
        /// <param name="values">The set of values.</param>
        /// 
        /// <returns>The minimum value, or the input if there is only one value.</returns>
        /// 
        /// <exception cref="System.ArgumentException">Thrown if <see cref="values"/> is empty.</exception>
        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            int count = values.Length;
            if (count == 0) throw new ArgumentException("You must provide at least one value");
            T currMin = values[0];
            for (int i = 1; i < count; ++i)
            {
                T num = values[i];
                if (num.CompareTo(currMin) == -1) currMin = num;
            }
            return currMin;
        }

        /// <summary>
        /// Finds the maximum value in a set of values.
        /// </summary>
        /// 
        /// <param name="values">The set of values.</param>
        /// 
        /// <returns>The maximum value, or the input if there is only one value.</returns>
        /// 
        /// <exception cref="System.ArgumentException">Thrown if <see cref="values"/> is empty.</exception>
        public static T Max<T>(params T[] values) where T : IComparable<T>
        {
            int count = values.Length;
            if (count == 0) throw new ArgumentException("You must provide at least one value");
            T currMax = values[0];
            for (int i = 1; i < count; ++i)
            {
                T num = values[i];
                if (num.CompareTo(currMax) == 1) currMax = num;
            }
            return currMax;
        }
    }
}