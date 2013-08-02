using System;
using System.Text;
using System.Windows.Forms;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for displaying and creating text.
    /// </summary>
    public static class TextUtilities
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
        /// Returns "null" if the object is null; calls the object's ToString method otherwise.
        /// </summary>
        /// <param name="obj">The object to convert to a string.</param>
        /// <returns>The string representation of the object.</returns>
        public static string ObjectToStringOrNull<T>(T obj)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return obj == null ? "null" : obj.ToString();
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
    }
}