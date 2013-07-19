using System;
using System.Drawing;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for getting random values.
    /// </summary>
    public static class RandomUtilities
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Gets a random color with opacity set to 255 (fully opaque).
        /// </summary>
        /// 
        /// <returns>An instance of <see cref="System.Drawing.Color"/> with random RGB values.</returns>
        public static Color ArgbColor()
        {
            return Color.FromArgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));
        }

        /// <summary>
        /// Gets a random color with the specified alpha value.
        /// </summary>
        /// 
        /// <param name="alpha">The alpha of the color.</param>
        /// 
        /// <returns>An instance of <see cref="System.Drawing.Color"/> with random RGB values.</returns>
        public static Color ArgbColor(int alpha)
        {
            return Color.FromArgb(alpha, Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));
        }

        /// <summary>
        /// Gets a pseudo-random non-negative integer less than the specified maximum.
        /// </summary>
        /// <param name="upperBound">The exclusive maximum value.</param>
        /// <returns>The generated random value.</returns>
        public static int Integer(int upperBound)
        {
            return Rnd.Next(upperBound);
        }

        /// <summary>
        /// Gets a pseudo-random integer within a specified range.
        /// </summary>
        /// 
        /// <param name="lowerBound">The inclusive lower bound of the number returned.</param>
        /// <param name="upperBound">The exclusive upper bound of the number returned.</param>
        /// 
        /// <returns>The generated random value.</returns>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if lowerBound is greater than upperBound.
        /// </exception>
        public static int Integer(int lowerBound, int upperBound)
        {
            return Rnd.Next(lowerBound, upperBound);
        }

        /// <summary>
        /// Gets a pseudo-random double within a specified range.
        /// </summary>
        /// 
        /// <param name="lowerBound">The inclusive lower bound of the number returned.</param>
        /// <param name="upperBound">The exclusive upper bound of the number returned.</param>
        /// 
        /// <returns>The generated random value.</returns>
        public static double Double(double lowerBound, double upperBound)
        {
            return Rnd.NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        /// <summary>
        /// Gets a pseudo-random float within a specified range.
        /// </summary>
        /// 
        /// <param name="lowerBound">The inclusive lower bound of the number returned.</param>
        /// <param name="upperBound">The exclusive upper bound of the number returned.</param>
        /// 
        /// <returns>The generated random value.</returns>
        public static float Single(float lowerBound, float upperBound)
        {
            return (float)Rnd.NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        /// <summary>
        /// Generates an outcome (true or false) based on a probability of getting true.
        /// </summary>
        /// 
        /// <param name="probability">The probability of getting true (0-100).</param>
        /// 
        /// <returns>True or false, depending on the outcome of the roll.</returns>
        public static bool BinaryOutcome(int probability)
        {
            return probability > Rnd.Next(100);
        }

        /// <summary>
        /// Generates an outcome (true or false) based on a probability of getting true.
        /// </summary>
        /// 
        /// <param name="probability">The probability of getting true (0-1).</param>
        /// 
        /// <returns>True or false, depending on the outcome of the roll.</returns>
        public static bool BinaryOutcome(double probability)
        {
            return probability > Rnd.NextDouble();
        }

        /// <summary>
        /// Gets a random combination of a specified number of items in the array.
        /// </summary>
        /// 
        /// <param name="count">The number of values to get.</param>
        /// <param name="array">The array containing the values.</param>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// 
        /// <returns>An array containing the random values.</returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if count is negative or greater than the number of items in the array.
        /// </exception>
        public static T[] Permutation<T>(int count, params T[] array)
        {
            if (count < 0 || count > array.Length) throw new ArgumentOutOfRangeException("count");

            T[] randomized = new T[count];

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