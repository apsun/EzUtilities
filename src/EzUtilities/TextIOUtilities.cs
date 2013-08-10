using System;
using System.Text;
using System.Windows.Forms;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for displaying and creating text.
    /// </summary>
    public static class TextIOUtilities
    {
        /// <summary>
        /// Shows a MessageBox with the specified message and the information icon.
        /// </summary>
        /// <param name="msg">The text to display on the MessageBox.</param>
        public static void MsgBox(object msg)
        {
            MsgBox(msg, string.Empty);
        }

        /// <summary>
        /// Shows a MessageBox with the specified message and icon.
        /// </summary>
        /// <param name="msg">The text to display on the MessageBox.</param>
        /// <param name="icon">The icon to display on the MessageBox.</param>
        public static void MsgBox(object msg, MessageBoxIcon icon)
        {
            MsgBox(msg, string.Empty, icon);
        }

        /// <summary>
        /// Shows a MessageBox with the specified message, title, and the information icon.
        /// </summary>
        /// <param name="msg">The text to display on the MessageBox.</param>
        /// <param name="title">The title of the MessageBox.</param>
        public static void MsgBox(object msg, object title)
        {
            MsgBox(msg, title, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows a MessageBox with the specified message, title, and icon.
        /// </summary>
        /// <param name="msg">The text to display on the MessageBox.</param>
        /// <param name="title">The title of the MessageBox.</param>
        /// <param name="icon">The icon to display on the MessageBox.</param>
        public static void MsgBox(object msg, object title, MessageBoxIcon icon)
        {
            MessageBox.Show(msg.ToString(), title.ToString(), MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Creates a string representation of the items in an array separated by a 
        /// comma followed by a space: { "a", "b", "c" }.
        /// </summary>
        /// <param name="array">The array to display.</param>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <returns>The items in the array joined by the specified separator.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        public static string ArrayToString<T>(T[] array)
        {
            return ArrayToString(", ", array);
        }

        /// <summary>
        /// Creates a string representation of the items in an array.
        /// </summary>
        /// <param name="separator">The separator to display between items.</param>
        /// <param name="array">The array to display.</param>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <returns>The items in the array joined by the specified separator.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        public static string ArrayToString<T>(string separator, T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var sb = new StringBuilder("{ ");

            if (array.Length > 0)
            {
                sb.Append(ObjectToStringOrNull(array[0]));

                for (int i = 1; i < array.Length; ++i)
                {
                    sb.Append(separator);
                    sb.Append(ObjectToStringOrNull(array[i]));
                }

                sb.Append(" }");
            }
            else
            {
                sb.Append('}');
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Prompts the user to enter a character to the console.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <param name="validOptions">
        /// An array of accepted input characters. 
        /// If this is null or empty, all input is accepted.
        /// </param>
        /// <returns>The first valid input the user provides, normalized to uppercase.</returns>
        public static char PromptChar(string question, params char[] validOptions)
        {
            bool allValid = validOptions.IsNullOrEmpty();

            if (!allValid)
            {
                for (int i = 0; i < validOptions.Length; ++i)
                {
                    validOptions[i] = char.ToUpperInvariant(validOptions[i]);
                }
            }

            while (true)
            {
                Console.Write(question);
                char input, inputUpper;
                do
                {
                    input = Console.ReadKey(true).KeyChar;
                    inputUpper = char.ToUpperInvariant(input);
                    if (allValid || Array.IndexOf(validOptions, inputUpper) >= 0) break;
                } while (true);
                Console.WriteLine(input);
                return inputUpper;
            }
        }

        /// <summary>
        /// Prompts the user to enter a string to the console.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <returns>The string entered by the user.</returns>
        public static string PromptString(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        /// <summary>
        /// Prompts the user to answer a yes/no question using the console.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <returns>True if the user selected 'yes'; false if the user selected 'no'.</returns>
        public static bool PromptYesNo(string question)
        {
            return PromptChar(question + " (y/n): ", 'Y', 'N') == 'Y';
        }

        /// <summary>
        /// Prompts the user to select an enum value using the console.
        /// </summary>
        /// <param name="question">The question to ask the user.</param>
        /// <typeparam name="T">An enum type. The enum must have 10 or fewer values.</typeparam>
        /// <returns>The selected enum value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum or has more than 10 values.
        /// </exception>
        public static T PromptSmallEnum<T>(string question) where T : struct
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum) throw new ArgumentException("Type is not an enum");

            string[] enumNames = Enum.GetNames(enumType);
            if (enumNames.Length > 10) throw new ArgumentException("Enum has more than 10 values");

            Console.WriteLine(question);

            var enumNums = new char[enumNames.Length];
            for (int i = 0; i < enumNames.Length; ++i)
            {
                var numChar = (char)((i + 1) % 10 + '0');
                enumNums[i] = numChar;
                Console.WriteLine(numChar + ". " + enumNames[i]);
            }

            char c = PromptChar("Select an option: ", enumNums);
            int numC = ((c - '0') - 1).PositiveMod(10);
            return (T)Enum.Parse(enumType, enumNames[numC]);
        }

        /// <summary>
        /// Pauses the console output until the user presses a key.
        /// </summary>
        public static void PauseConsole()
        {
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        /// <summary>
        /// Returns "null" if the object is null; calls the object's ToString method otherwise.
        /// </summary>
        /// <param name="obj">The object to convert to a string.</param>
        /// <returns>The string representation of the object.</returns>
        private static string ObjectToStringOrNull<T>(T obj)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return obj == null ? "null" : obj.ToString();
        }
    }
}