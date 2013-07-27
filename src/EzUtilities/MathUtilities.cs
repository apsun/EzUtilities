using System;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for comparing numbers.
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// The single-precision epsilon, equal to 2^-23 (1.19209290E-07).
        /// </summary>
        public static readonly float SingleEpsilon = (float)Math.Pow(2, -23);

        /// <summary>
        /// The double-precision epsilon, equal to 2^-52 (2.2204460492503131E-16).
        /// </summary>
        public static readonly double DoubleEpsilon = Math.Pow(2, -52);

        /// <summary>
        /// Checks whether two single-precision floating point numbers 
        /// are approximately equal to each other.
        /// </summary>
        /// 
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <param name="epsilon">The tolerance for differences in values.</param>
        /// 
        /// <returns>Whether the two floats are approximately equal.</returns>
        public static bool ApproxEquals(float a, float b, float epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// Checks whether two single-precision floating point numbers 
        /// are approximately equal to each other using the default 
        /// tolerance epsilon (1000*2^-23).
        /// </summary>
        /// 
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// 
        /// <returns>Whether the two floats are approximately equal.</returns>
        public static bool ApproxEquals(this float a, float b)
        {
            return ApproxEquals(a, b, 1000 * SingleEpsilon);
        }

        /// <summary>
        /// Checks whether two double-precision floating point numbers 
        /// are approximately equal to each other.
        /// </summary>
        /// 
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <param name="epsilon">The tolerance for differences in values.</param>
        /// 
        /// <returns>Whether the two doubles are approximately equal.</returns>
        public static bool ApproxEquals(double a, double b, double epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// Checks whether two double-precision floating point numbers 
        /// are approximately equal to each other using the default 
        /// tolerance epsilon (1000*2^-52).
        /// </summary>
        /// 
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// 
        /// <returns>Whether the two doubles are approximately equal.</returns>
        public static bool ApproxEquals(this double a, double b)
        {
            return ApproxEquals(a, b, 1000 * DoubleEpsilon);
        }

        /// <summary>
        /// Finds the minimum value in a set of values.
        /// </summary>
        /// 
        /// <param name="values">The set of values.</param>
        /// 
        /// <returns>The minimum value, or the input if there is only one value.</returns>
        /// 
        /// <exception cref="System.ArgumentException">Thrown if values is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if values is null.</exception>
        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            if (values == null) throw new ArgumentNullException("values");
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
        /// <exception cref="System.ArgumentException">Thrown if values is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if values is null.</exception>
        public static T Max<T>(params T[] values) where T : IComparable<T>
        {
            if (values == null) throw new ArgumentNullException("values");
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

        /// <summary>
        /// Checks whether the value is within a range, inclusive.
        /// </summary>
        /// 
        /// <param name="value">The value to check.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="higher">The inclusive upper bound.</param>
        /// 
        /// <returns>Whether the value is within the range.</returns>
        /// 
        /// <exception cref="ArgumentException">Thrown if the lower bound is greater than the upper bound.</exception>
        public static bool IsBetween<T>(this T value, T lower, T higher) where T : IComparable<T>
        {
            if (lower.CompareTo(higher) == 1)
            {
                throw new ArgumentException("Upper bound must be greater than lower bound");
            }
            return value.CompareTo(lower) >= 0 && value.CompareTo(higher) <= 0;
        }

        /// <summary>
        /// Finds the minimum and maximum of a pair of values.
        /// </summary>
        /// 
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="min">The lesser value.</param>
        /// <param name="max">The greater value.</param>
        /// 
        /// <returns>
        /// -1 if value1 is less than value2, 
        /// 0 if value1 equals value2, 
        /// 1 if value1 is greater than value2.
        /// </returns>
        public static int GetMinMax<T>(T value1, T value2, out T min, out T max) where T : IComparable<T>
        {
            int returnValue = value1.CompareTo(value2);

            if (returnValue == -1) //value1 < value2
            {
                min = value1;
                max = value2;
            }
            else //value1 >= value2
            {
                min = value2;
                max = value1;
            }

            return returnValue;
        }

        /// <summary>
        /// Wraps a number within a range of [lower, upper).
        /// </summary>
        /// 
        /// <param name="num">The number to wrap.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The exclusive upper bound.</param>
        /// 
        /// <returns>The wrapped number.</returns>
        /// 
        /// <exception cref="ArgumentException">Thrown if lower is greater than upper.</exception>
        public static int Wrap(this int num, int lower, int upper)
        {
            if (upper <= lower) throw new ArgumentException("upper must be greater than lower");

            int t = (num - lower) % (upper - lower);

            return t < 0 ? t + upper : t + lower;
        }

        /// <summary>
        /// Wraps a number within a range of [lower, upper).
        /// </summary>
        /// 
        /// <param name="num">The number to wrap.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The exclusive upper bound.</param>
        /// 
        /// <returns>The wrapped number.</returns>
        /// 
        /// <exception cref="ArgumentException">Thrown if lower is greater than upper.</exception>
        public static float Wrap(this float num, float lower, float upper)
        {
            if (upper <= lower) throw new ArgumentException("upper must be greater than lower");

            float t = (num - lower) % (upper - lower);

            return t < 0 ? t + upper : t + lower;
        }

        /// <summary>
        /// Wraps a number within a range of [lower, upper).
        /// </summary>
        /// 
        /// <param name="num">The number to wrap.</param>
        /// <param name="lower">The inclusive lower bound.</param>
        /// <param name="upper">The exclusive upper bound.</param>
        /// 
        /// <returns>The wrapped number.</returns>
        /// 
        /// <exception cref="ArgumentException">Thrown if lower is greater than upper.</exception>
        public static double Wrap(this double num, double lower, double upper)
        {
            double t = (num - lower) % (upper - lower);

            return t < 0 ? t + upper : t + lower;
        }
    }
}