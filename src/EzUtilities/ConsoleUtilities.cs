using System;
using System.IO;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for capturing input via the console.
    /// </summary>
    public static class ConsoleUtilities
    {
        /// <summary>
        /// Prompts the user to enter a string.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <returns>The string entered by the user.</returns>
        public static string PromptString(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        /// <summary>
        /// Prompts the user to enter a string.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="invalidResponse">
        /// The message to display when the user enters invalid input. 
        /// If this value is null, an exception will be thrown if the 
        /// user enters invalid input.
        /// </param>
        /// <param name="validator">
        /// A function that validates the user's input. 
        /// Returns true if the input is valid; false otherwise.
        /// </param>
        /// <returns>The string entered by the user.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if validator is null.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if invalidResponse is null and the user enters invalid input.
        /// </exception>
        public static string PromptString(string question, string invalidResponse, Func<string, bool> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            while (true)
            {
                string input = PromptString(question);
                if (validator(input)) return input;
                if (invalidResponse == null)
                    throw new IOException("User did not enter a valid string");
                if (invalidResponse != string.Empty)
                    Console.WriteLine(invalidResponse);
            }
        }

        /// <summary>
        /// Prompts the user to enter a character.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="validOptions">
        /// An array of accepted input characters. 
        /// If this is null or empty, all input is accepted.
        /// </param>
        /// <returns>The first valid character the user enters, normalized to uppercase.</returns>
        public static char PromptChar(string question, params char[] validOptions)
        {
            char[] validUpper = null;
            if (!validOptions.IsNullOrEmpty())
            {
                validUpper = new char[validOptions.Length];
                for (int i = 0; i < validOptions.Length; ++i)
                {
                    validUpper[i] = char.ToUpperInvariant(validOptions[i]);
                }
            }

            return PromptChar(question, c => validUpper == null || Array.IndexOf(validUpper, c) >= 0);
        }

        /// <summary>
        /// Prompts the user to enter a character.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="validator">
        /// A function that validates the user's input. 
        /// Returns true if the input is valid; false otherwise.
        /// </param>
        /// <returns>The first valid character the user enters, normalized to uppercase.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if validator is null.
        /// </exception>
        public static char PromptChar(string question, Func<char, bool> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            while (true)
            {
                Console.Write(question);
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
        /// Prompts the user for a 32-bit integer.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="invalidResponse">
        /// The message to display when the user enters invalid input. 
        /// If this value is null, an exception will be thrown if the 
        /// user enters invalid input.
        /// </param>
        /// <returns>The integer the user entered.</returns>
        /// <exception cref="IOException">
        /// Thrown if invalidResponse is null and the user enters invalid input.
        /// </exception>
        public static int PromptInteger(string question, string invalidResponse)
        {
            while (true)
            {
                string input = PromptString(question);
                int value;
                if (int.TryParse(input, out value)) return value;
                if (invalidResponse == null)
                    throw new IOException("User did not enter a valid integer");
                if (invalidResponse != string.Empty)
                    Console.WriteLine(invalidResponse);
            }
        }

        /// <summary>
        /// Prompts the user for a 32-bit integer within a specified range.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="invalidResponse">
        /// The message to display when the user enters invalid input. 
        /// If this value is null, an exception will be thrown if the 
        /// user enters invalid input.
        /// </param>
        /// <param name="min">The inclusive lower bound of the accepted range.</param>
        /// <param name="max">The inclusive upper bound of the accepted range.</param>
        /// <returns>The integer the user entered.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the upper bound is not greater than the minimum bound.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if invalidResponse is null and the user enters invalid input 
        /// or a number outside the allowed range.
        /// </exception>
        public static int PromptInteger(string question, string invalidResponse, int min, int max)
        {
            if (min >= max) throw new ArgumentException("max must be greater than min");
            return PromptInteger(question, invalidResponse, i => i >= min && i <= max);
        }

        /// <summary>
        /// Prompts the user for a 32-bit integer within a specified range.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="invalidResponse">
        /// The message to display when the user enters invalid input. 
        /// If this value is null, an exception will be thrown if the 
        /// user enters invalid input.
        /// </param>
        /// <param name="validator">
        /// A function that validates the user's input. 
        /// Returns true if the input is valid; false otherwise.
        /// </param>
        /// <returns>The integer the user entered.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if validator is null.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if invalidResponse is null and the user enters invalid input 
        /// or a number outside the allowed range.
        /// </exception>
        public static int PromptInteger(string question, string invalidResponse, Func<int, bool> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            while (true)
            {
                int value = PromptInteger(question, invalidResponse);
                if (validator(value)) return value;
                if (invalidResponse == null)
                    throw new IOException("User entered an integer outside the allowed range");
                if (invalidResponse != string.Empty)
                    Console.WriteLine(invalidResponse);
            }
        }

        /// <summary>
        /// Prompts the user to answer a yes/no question.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="yes">The input that is accepted as 'yes'.</param>
        /// <param name="no">The input that is accepted as 'no'.</param>
        /// <returns>True if the user selected 'yes'; false if the user selected 'no'.</returns>
        public static bool PromptYesNo(string question, char yes, char no)
        {
            string q = string.Format("{0} ({1}/{2}): ", question, yes, no);
            return PromptChar(q, yes, no) == char.ToUpper(yes);
        }

        /// <summary>
        /// Prompts the user to select an enum value.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <typeparam name="T">An enum type. The enum must have between 1 and 10 values.</typeparam>
        /// <returns>The selected enum value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum or does not have between 1 and 10 values.
        /// </exception>
        public static T PromptShortEnum<T>(string question)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum) throw new ArgumentException("Type is not an enum");

            string[] enumNames = Enum.GetNames(enumType);
            if (enumNames.Length == 0) throw new ArgumentException("Enum has no values");
            if (enumNames.Length > 10) throw new ArgumentException("Enum has more than 10 values");

            var enumNums = new char[enumNames.Length];
            for (int i = 0; i < enumNames.Length; ++i)
            {
                var numChar = (char)((i + 1) % 10 + '0');
                enumNums[i] = numChar;
                Console.WriteLine(numChar + ". " + enumNames[i]);
            }

            char c = PromptChar(question, enumNums);
            int numC = (c - '0' + 9) % 10;
            return (T)Enum.Parse(enumType, enumNames[numC]);
        }

        /// <summary>
        /// Prompts the user to select an enum value.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="invalidResponse">The message to display when the user enters invalid input.</param>
        /// <typeparam name="T">An enum type. The enum must have at least one value.</typeparam>
        /// <returns>The selected enum value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum or has no values.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if invalidResponse is null and the user enters invalid input.
        /// </exception>
        public static T PromptLongEnum<T>(string question, string invalidResponse)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum) throw new ArgumentException("Type is not an enum");

            string[] enumNames = Enum.GetNames(enumType);
            if (enumNames.Length == 0) throw new ArgumentException("Enum has no values");

            for (int i = 0; i < enumNames.Length; ++i)
            {
                Console.WriteLine((i + 1) + ". " + enumNames[i]);
            }

            int num = PromptInteger(question, invalidResponse, 1, enumNames.Length);
            return (T)Enum.Parse(enumType, enumNames[num - 1]);
        }
    }
}