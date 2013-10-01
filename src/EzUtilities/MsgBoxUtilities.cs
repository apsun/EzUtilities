using System.Windows.Forms;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for displaying message boxes.
    /// </summary>
    public static class MsgBoxUtilities
    {
        /// <summary>
        /// Shows a message box with the specified message.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        public static void MsgBox(object msg)
        {
            MsgBox(msg, string.Empty);
        }

        /// <summary>
        /// Shows a message box with the specified message and title.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <param name="title">The title of the message box.</param>
        public static void MsgBox(object msg, object title)
        {
            MsgBox(msg, title, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows a message box with the specified message and icon.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <param name="icon">The icon to display on the message box.</param>
        public static void MsgBox(object msg, MessageBoxIcon icon)
        {
            MsgBox(msg, string.Empty, icon);
        }

        /// <summary>
        /// Shows a message box with the specified message, title, and icon.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <param name="title">The title of the message box.</param>
        /// <param name="icon">The icon to display on the message box.</param>
        public static void MsgBox(object msg, object title, MessageBoxIcon icon)
        {
            MessageBox.Show(msg.ToString(), title.ToString(), MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Shows a message box with the specified message 
        /// and prompts the user to select either 'yes' or 'no'.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <returns>True if the user pressed 'yes'; false otherwise.</returns>
        public static bool MsgBoxYesNo(object msg)
        {
            return MsgBoxYesNo(msg, string.Empty);
        }

        /// <summary>
        /// Shows a message box with the specified message and title 
        /// and prompts the user to select either 'yes' or 'no'.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <param name="title">The title of the message box.</param>
        /// <returns>True if the user pressed 'yes'; false otherwise.</returns>
        public static bool MsgBoxYesNo(object msg, object title)
        {
            return MsgBoxYesNo(msg, title, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Shows a message box with the specified message and icon 
        /// and prompts the user to select either 'yes' or 'no'.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <param name="icon">The icon to display on the message box.</param>
        /// <returns>True if the user pressed 'yes'; false otherwise.</returns>
        public static bool MsgBoxYesNo(object msg, MessageBoxIcon icon)
        {
            return MsgBoxYesNo(msg, string.Empty, icon);
        }

        /// <summary>
        /// Shows a message box with the specified message, title, and icon 
        /// and prompts the user to select either 'yes' or 'no'.
        /// </summary>
        /// <param name="msg">The text to display on the message box.</param>
        /// <param name="title">The title of the message box.</param>
        /// <param name="icon">The icon to display on the message box.</param>
        /// <returns>True if the user pressed 'yes'; false otherwise.</returns>
        public static bool MsgBoxYesNo(object msg, object title, MessageBoxIcon icon)
        {
            return MessageBox.Show(msg.ToString(), title.ToString(), MessageBoxButtons.YesNo, icon) == DialogResult.Yes;
        }
    }
}