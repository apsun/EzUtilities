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
        /// Gets a random color with the specified alpha value.
        /// </summary>
        /// 
        /// <param name="alpha">The alpha of the color.</param>
        /// 
        /// <returns>An instance of <see cref="System.Drawing.Color"/> with random RGB values.</returns>
        public static Color GetRandomColor(int alpha)
        {
            return Color.FromArgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256), alpha);
        }

        /// <summary>
        /// Gets a random integer within a specified range.
        /// </summary>
        /// 
        /// <param name="lowerBound">The inclusive lower bound of the number returned.</param>
        /// <param name="upperBound">The exclusive upper bound of the number returned.</param>
        /// 
        /// <returns>The generated random value.</returns>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if <see cref="lowerBound"/> is greater than <see cref="upperBound"/>.
        /// </exception>
        public static int GetRandomInteger(int lowerBound, int upperBound)
        {
            return Rnd.Next(lowerBound, upperBound);
        }

        /// <summary>
        /// Gets a random double within a specified range.
        /// </summary>
        /// 
        /// <param name="lowerBound">The inclusive lower bound of the number returned.</param>
        /// <param name="upperBound">The exclusive upper bound of the number returned.</param>
        /// 
        /// <returns>The generated random value.</returns>
        public static double GetRandomDouble(double lowerBound, double upperBound)
        {
            return Rnd.NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        /// <summary>
        /// Gets a random float within a specified range.
        /// </summary>
        /// 
        /// <param name="lowerBound">The inclusive lower bound of the number returned.</param>
        /// <param name="upperBound">The exclusive upper bound of the number returned.</param>
        /// 
        /// <returns>The generated random value.</returns>
        public static float GetRandomFloat(float lowerBound, float upperBound)
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
        public static bool GetBinaryOutcome(int probability)
        {
            return probability > Rnd.Next(100);
        }
    }
}