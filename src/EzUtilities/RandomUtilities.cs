using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for generating pseudo-random values.
    /// </summary>
    public static class RandomUtilities
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Gets a random color with an alpha value of 255 (fully opaque).
        /// </summary>
        public static Color ArgbColor()
        {
            return ArgbColor(255);
        }

        /// <summary>
        /// Gets a random color with a specified alpha value.
        /// </summary>
        /// <param name="alpha">The alpha value of the color (0-255).</param>
        public static Color ArgbColor(int alpha)
        {
            Contract.Requires<ArgumentOutOfRangeException>(alpha >= 0 && alpha < 256);
            return Color.FromArgb(alpha, Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));
        }

        /// <summary>
        /// Gets a random color with a specified alpha value.
        /// </summary>
        /// <param name="alpha">The alpha value of the color (0-1).</param>
        public static Color ArgbColor(double alpha)
        {
            Contract.Requires<ArgumentOutOfRangeException>(alpha >= 0 && alpha < 256);
            return ArgbColor((int)(alpha * 255));
        }

        /// <summary>
        /// Gets a pseudo-random non-negative integer less than the specified maximum.
        /// </summary>
        /// <param name="upperBound">The exclusive maximum value.</param>
        public static int Integer(int upperBound)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upperBound >= 0);
            return Rnd.Next(upperBound);
        }

        /// <summary>
        /// Gets a pseudo-random integer within a specified range.
        /// </summary>
        /// <param name="lowerBound">The inclusive lower bound of the number.</param>
        /// <param name="upperBound">The exclusive upper bound of the number.</param>
        public static int Integer(int lowerBound, int upperBound)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upperBound > lowerBound);
            return Rnd.Next(lowerBound, upperBound);
        }

        /// <summary>
        /// Gets a pseudo-random double within a specified range.
        /// </summary>
        /// <param name="lowerBound">The inclusive lower bound of the number.</param>
        /// <param name="upperBound">The exclusive upper bound of the number.</param>
        public static double Double(double lowerBound, double upperBound)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upperBound > lowerBound);
            return Rnd.NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        /// <summary>
        /// Gets a pseudo-random single within a specified range.
        /// </summary>
        /// <param name="lowerBound">The inclusive lower bound of the number.</param>
        /// <param name="upperBound">The exclusive upper bound of the number.</param>
        public static float Single(float lowerBound, float upperBound)
        {
            Contract.Requires<ArgumentOutOfRangeException>(upperBound > lowerBound);
            return (float)Rnd.NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        /// <summary>
        /// Generates an outcome (true/false) based on a probability of getting true.
        /// </summary>
        /// <param name="probability">The probability of getting true (0-100).</param>
        public static bool BinaryOutcome(int probability)
        {
            Contract.Requires<ArgumentOutOfRangeException>(probability >= 0 && probability <= 100);
            return probability > Rnd.Next(100);
        }

        /// <summary>
        /// Generates an outcome (true/false) based on a probability of getting true.
        /// </summary>
        /// <param name="probability">The probability of getting true (0-1).</param>
        public static bool BinaryOutcome(double probability)
        {
            Contract.Requires<ArgumentOutOfRangeException>(probability >= 0 && probability <= 1);
            return probability > Rnd.NextDouble();
        }

        /// <summary>
        /// Gets a random permutation of a specified number of items in the array.
        /// </summary>
        /// <param name="count">The number of values to get.</param>
        /// <param name="array">The array containing the values.</param>
        /// <typeparam name="T">The type of the array.</typeparam>
        public static T[] Permutation<T>(int count, params T[] array)
        {
            Contract.Requires<ArgumentNullException>(array != null);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0 && count <= array.Length);

            var randomized = new T[count];
            if (count == 0) return randomized;

            T[] copy = array.Copy();

            int n;
            int length = n = array.Length;
            while (n-- > length - count)
            {
                int k = Rnd.Next(n + 1);
                randomized[length - n - 1] = copy[k];
                copy[k] = copy[n];
            }

            return randomized;
        }
    }
}
