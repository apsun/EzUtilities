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
        /// Writes a message to the debug output window.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        public static void Print(object msg)
        {
            Debug.Print(msg.ToString());
        }
    }
}