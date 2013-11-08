using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace EzUtilities
{
    /// <summary>
    /// Provides console I/O utilities.
    /// </summary>
    public static class ConsoleUtilities
    {
        /// <summary>
        /// Prompts the user for a string.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        public static string PromptString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        /// <summary>
        /// Prompts the user for a string.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="invalidResponse">The message to display when the user enters an invalid value.</param>
        /// <param name="validator">A function to determine whether the user input is valid.</param>
        public static string PromptString(string prompt, string invalidResponse, Func<string, bool> validator)
        {
            Contract.Requires<ArgumentNullException>(validator != null);
            while (true)
            {
                string input = PromptString(prompt);
                if (validator(input)) return input;
                if (invalidResponse == null)
                    throw new IOException("User did not enter a valid string");
                if (invalidResponse != string.Empty)
                    Console.WriteLine(invalidResponse);
            }
        }

        /// <summary>
        /// Prompts the user for a single character.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="validChars">An array of accepted characters. The list is case-insensitive.</param>
        public static char PromptChar(string prompt, params char[] validChars)
        {
            Contract.Requires<ArgumentNullException>(validChars != null);
            Contract.Requires<ArgumentNullException>(validChars.Length > 0);
            var validUpper = new char[validChars.Length];
            for (int i = 0; i < validChars.Length; ++i)
                validUpper[i] = char.ToUpperInvariant(validChars[i]);
            return PromptChar(prompt, c => Array.IndexOf(validUpper, c) >= 0);
        }

        /// <summary>
        /// Prompts the user for a single character.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="validator">A function to determine whether the user input is valid.</param>
        public static char PromptChar(string prompt, Func<char, bool> validator)
        {
            Contract.Requires<ArgumentNullException>(validator != null);
            while (true)
            {
                Console.Write(prompt);
                char input, inputUpper;
                do
                {
                    input = Console.ReadKey(true).KeyChar;
                    inputUpper = char.ToUpperInvariant(input);
                    if (validator(inputUpper)) break;
                } while (true);
                Console.WriteLine(input);
                return inputUpper;
            }
        }

        /// <summary>
        /// Prompts the user for an integer.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="invalidResponse">The message to display when the user enters an invalid value.</param>
        public static int PromptInteger(string prompt, string invalidResponse)
        {
            while (true)
            {
                string input = PromptString(prompt);
                int value;
                if (int.TryParse(input, out value)) return value;
                if (invalidResponse == null)
                    throw new IOException("User did not enter a valid integer");
                if (invalidResponse != string.Empty)
                    Console.WriteLine(invalidResponse);
            }
        }

        /// <summary>
        /// Prompts the user for an integer within the range of [min, max].
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="invalidResponse">The message to display when the user enters an invalid value.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        public static int PromptInteger(string prompt, string invalidResponse, int min, int max)
        {
            Contract.Requires<ArgumentOutOfRangeException>(max >= min);
            return PromptInteger(prompt, invalidResponse, i => i >= min && i <= max);
        }

        /// <summary>
        /// Prompts the user for an integer within the range of [min, max].
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="invalidResponse">The message to display when the user enters an invalid value.</param>
        /// <param name="validator">A function to determine whether the user input is valid.</param>
        public static int PromptInteger(string prompt, string invalidResponse, Func<int, bool> validator)
        {
            Contract.Requires<ArgumentNullException>(validator != null);
            while (true)
            {
                int value = PromptInteger(prompt, invalidResponse);
                if (validator(value)) return value;
                if (invalidResponse == null)
                    throw new IOException("User entered an integer outside the allowed range");
                if (invalidResponse != string.Empty)
                    Console.WriteLine(invalidResponse);
            }
        }

        /// <summary>
        /// Prompts the user for an boolean value (true/false).
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="trueChar">The character that represents a true value.</param>
        /// <param name="falseChar">The character that represents a false value.</param>
        public static bool PromptBoolean(string prompt, char trueChar, char falseChar)
        {
            string q = string.Format("{0} ({1}/{2}): ", prompt, trueChar, falseChar);
            return PromptChar(q, trueChar, falseChar) == char.ToUpper(trueChar);
        }

        /// <summary>
        /// Prompts the user to select an enum value. 
        /// The enum must have ten or fewer values.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="prompt">The prompt to display to the user.</param>
        public static T PromptShortEnum<T>(string prompt)
        {
            Type enumType = typeof(T);
            Contract.Requires<ArgumentException>(enumType.IsEnum);
            string[] enumNames = Enum.GetNames(enumType);
            Contract.Requires<ArgumentException>(enumNames.Length > 0);
            Contract.Requires<ArgumentException>(enumNames.Length <= 10);

            var enumNums = new char[enumNames.Length];
            for (int i = 0; i < enumNames.Length; ++i)
            {
                var numChar = (char)((i + 1) % 10 + '0');
                enumNums[i] = numChar;
                Console.WriteLine(numChar + ". " + enumNames[i]);
            }

            char c = PromptChar(prompt, enumNums);
            int numC = (c - '0' + 9) % 10;
            return (T)Enum.Parse(enumType, enumNames[numC]);
        }

        /// <summary>
        /// Prompts the user to select an enum value.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="invalidResponse">The message to display when the user enters an invalid value.</param>
        public static T PromptLongEnum<T>(string prompt, string invalidResponse)
        {
            Type enumType = typeof(T);
            Contract.Requires<ArgumentException>(enumType.IsEnum);
            string[] enumNames = Enum.GetNames(enumType);
            Contract.Requires<ArgumentException>(enumNames.Length > 0);

            for (int i = 0; i < enumNames.Length; ++i)
                Console.WriteLine((i + 1) + ". " + enumNames[i]);

            int num = PromptInteger(prompt, invalidResponse, 1, enumNames.Length);
            return (T)Enum.Parse(enumType, enumNames[num - 1]);
        }
    }
}