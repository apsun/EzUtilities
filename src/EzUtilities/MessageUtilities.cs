using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for displaying text.
    /// </summary>
    public static class MessageUtilities
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
        /// Writes the contents of an array to the debug output window, 
        /// using a comma and space (", ") as the delimiter.
        /// </summary>
        /// <param name="array">The array to print.</param>
        public static void DebugPrintArray<T>(params T[] array)
        {
            DebugPrintArray(", ", array);
        }

        /// <summary>
        /// Writes the contents of an array to the debug output window.
        /// </summary>
        /// <param name="separator">The separator between items in the array.</param>
        /// <param name="array">The array to print.</param>
        public static void DebugPrintArray<T>(string separator, params T[] array)
        {
            Debug.WriteLine(string.Join(separator, array));
        }

        /// <summary>
        /// Writes the contents of an array to the console window, 
        /// using a comma and space (", ") as the delimiter.
        /// </summary>
        /// <param name="array">The array to print.</param>
        public static void ConsolePrintArray<T>(params T[] array)
        {
            ConsolePrintArray(", ", array);
        }

        /// <summary>
        /// Writes the contents of an array to the console window.
        /// </summary>
        /// <param name="separator">The separator between items in the array.</param>
        /// <param name="array">The array to print.</param>
        public static void ConsolePrintArray<T>(string separator, params T[] array)
        {
            Console.WriteLine(string.Join(separator, array));
        }
    }
}