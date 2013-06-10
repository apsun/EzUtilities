using System;

namespace EzUtilities
{
    public static class MathUtilities
    {
        public const float FloatEqualityEpsilon = 0.00001f;
        public const double DoubleEqualityEpsilon = 0.0000000001d;

        /// <summary>
        /// Checks whether two floats are approximately equal to each other. 
        /// May return incorrect results when an overflow occurs.
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <param name="epsilon">The tolerance for differences in values.</param>
        /// <returns></returns>
        public static bool ApproxEquals(this float a, float b, float epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// Checks whether two floats are approximately equal to each other 
        /// using the default tolerance epsilon (1E-05).
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <returns></returns>
        public static bool ApproxEquals(this float a, float b)
        {
            return ApproxEquals(a, b, FloatEqualityEpsilon);
        }

        /// <summary>
        /// Checks whether two doubles are approximately equal to each other.
        /// </summary>
        /// <param name="a">The first double.</param>
        /// <param name="b">The second double.</param>
        /// <param name="epsilon">The tolerance for differences in values.</param>
        /// <returns></returns>
        public static bool ApproxEquals(this double a, double b, double epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// Checks whether two doubles are approximately equal to each other 
        /// using the default tolerance epsilon (1E-10).
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <returns></returns>
        public static bool ApproxEquals(this double a, double b)
        {
            return ApproxEquals(a, b, DoubleEqualityEpsilon);
        }

        /// <summary>
        /// Finds the minimum value in a set of values.
        /// </summary>
        /// <param name="values">The set of values.</param>
        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            int count = values.Length;
            if (count == 0) throw new ArgumentException("nums");
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
        /// <param name="values">The set of values.</param>
        public static T Max<T>(params T[] values) where T : IComparable<T>
        {
            int count = values.Length;
            if (count == 0) throw new ArgumentException("nums");
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