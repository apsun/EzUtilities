using System;
using System.Globalization;
using System.IO;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to work with directories, files, and paths.
    /// </summary>
    public static class IOUtilities
    {
        /// <summary>
        /// Characters considered as whitespace that can be 
        /// safely removed from the ends of paths.
        /// </summary>
        private static readonly char[] WhitespaceChars =
        { ' ', '\t', '\n', '\v', '\f', '\r', '\x0085' };

        /// <summary>
        /// Creates a directory, doing nothing if the directory already exists.
        /// </summary>
        /// 
        /// <param name="path">The path of the directory to create.</param>
        /// 
        /// <returns>
        /// Whether the directory was created; false if the 
        /// creation failed or the directory already exists.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">Thrown if path is null.</exception>
        public static bool CreateDirectory(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (Directory.Exists(path)) return false;

            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the directory that contains this file or directory. 
        /// </summary>
        /// 
        /// <param name="path">The path to the file or directory.</param>
        /// 
        /// <returns>
        /// Returns false if the parent directory already exists; otherwise, true.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentException">path is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.-or- The system could not retrieve the absolute path. </exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permissions. </exception>
        /// <exception cref="System.ArgumentNullException">path is null. </exception>
        /// <exception cref="System.NotSupportedException">path contains a colon (":") that is not part of a volume identifier (for example, "c:\"). </exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        public static bool CreateParentDirectory(string path)
        {
            string parentDir = GetParentDirectory(path);

            if (Directory.Exists(parentDir)) return false;

            Directory.CreateDirectory(parentDir);

            return true;
        }

        /// <summary>
        /// Deletes a directory and all subdirectories and files.
        /// </summary>
        /// 
        /// <param name="path">The path of the directory to delete.</param>
        /// 
        /// <returns>Whether the directory was successfully deleted.</returns>
        /// 
        /// <exception cref="System.ArgumentNullException">Thrown if path is null.</exception>
        public static bool DeleteDirectory(string path)
        {
            return DeleteDirectory(path, true);
        }

        /// <summary>
        /// Deletes a directory, and if indicated, all subdirectories and files in the directory.
        /// </summary>
        /// 
        /// <param name="path">The path of the directory to delete.</param>
        /// <param name="recursive">Whether to delete subdirectories and files in the directory.</param>
        /// 
        /// <returns>Whether the directory was successfully deleted.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        public static bool DeleteDirectory(string path, bool recursive)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!Directory.Exists(path)) return false;

            try
            {
                Directory.Delete(path, recursive);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// 
        /// <param name="path">The path of the file to delete.</param>
        /// 
        /// <returns>Whether the file was successfully deleted.</returns>
        /// 
        /// <exception cref="System.ArgumentNullException">path</exception>
        public static bool DeleteFile(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!File.Exists(path)) return false;

            try
            {
                File.Delete(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Moves a file, optionally overwriting the destination file if it exists. 
        /// Returns whether the destination file was overwritten. 
        /// Throws an exception if the destination file exists but overwrite is set to false.
        /// </summary>
        /// 
        /// <param name="srcPath">The path of the file to move.</param>
        /// <param name="destPath">The new path of the file.</param>
        /// <param name="overwrite">Whether to overwrite existing files.</param>
        /// 
        /// <returns>Whether the destination file was overwritten, if it existed.</returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if srcPath or destPath is null.
        /// </exception>
        /// 
        /// <exception cref="System.IO.IOException">
        /// Thrown if the destination file already exists and overwrite is false, 
        /// or if the source file was not found.
        /// </exception>
        /// 
        /// <exception cref="System.ArgumentException">sourceFileName or destFileName is a zero-length string, contains only white space, or contains invalid characters. </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The path specified in sourceFileName or destFileName is invalid, (for example, it is on an unmapped drive). </exception>
        /// <exception cref="System.NotSupportedException">sourceFileName or destFileName is in an invalid format. </exception>
        public static bool MoveFile(string srcPath, string destPath, bool overwrite)
        {
            if (srcPath == null) throw new ArgumentNullException("srcPath");
            if (destPath == null) throw new ArgumentNullException("destPath");
            bool deletedDest = false;
            if (overwrite)
            {
                deletedDest = DeleteFile(destPath);
            }
            File.Move(srcPath, destPath);
            return deletedDest;
        }

        /// <summary>
        /// Deletes all files and directories in the specified directory. 
        /// Returns false if the path is not a directory, the directory 
        /// does not exist, or if any items were not successfully deleted.
        /// </summary>
        /// 
        /// <param name="path">The path of the directory to empty.</param>
        /// 
        /// <returns>Whether all items within the directory were deleted.</returns>
        public static bool EmptyDirectory(string path)
        {
            if (!Directory.Exists(path)) return false;

            bool allItemsDeleted = true;
            foreach (var file in Directory.GetFiles(path))
            {
                if (!DeleteFile(file)) allItemsDeleted = false;
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                if (!DeleteDirectory(dir)) allItemsDeleted = false;
            }

            return allItemsDeleted;

        }

        /// <summary>
        /// Gets the parent directory of the specified file or directory.
        /// </summary>
        /// 
        /// <param name="path">The path of the file or directory.</param>
        /// 
        /// <returns>The path of the parent directory.</returns>
        /// 
        /// <exception cref="ArgumentException">Thrown if path is a root path.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if path is null.</exception>
        public static string GetParentDirectory(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (path.Length > 3) path = RemoveTrailingSeparator(path);
            string parent = Path.GetDirectoryName(path);
            if (parent == null) throw new ArgumentException("Path is a root path");
            return parent;
        }

        /// <summary>
        /// Gets a random file name without an extension.
        /// </summary>
        public static string GetRandomFileName()
        {
            return Path.ChangeExtension(Path.GetRandomFileName(), null);
        }

        /// <summary>
        /// Removes all whitespace from the beginning of the path.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        public static string RemoveLeadingWhitespace(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            return path.TrimStart(WhitespaceChars);
        }

        /// <summary>
        /// Removes all whitespace from the end of the path.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        public static string RemoveTrailingWhitespace(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            return path.TrimEnd(WhitespaceChars);
        }

        /// <summary>
        /// Removes all whitespace from the beginning and end of the path.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if path is null.</exception>
        public static string RemoveLeadingAndTrailingWhitespace(string path)
        {
            return RemoveLeadingWhitespace(RemoveTrailingWhitespace(path));
        }

        /// <summary>
        /// Prepends a directory separator to the beginning of the path if there is not one already. 
        /// Returns the directory separator if path is empty or null.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if path is not relative.</exception>
        public static string PrependSeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

            path = RemoveLeadingWhitespace(path);

            if (IsPathAbsolute(path)) throw new ArgumentException("Path is not relative");

            if (path.IndexOf(Path.DirectorySeparatorChar) == 0) return path;
            if (path.IndexOf(Path.AltDirectorySeparatorChar) == 0) return path;

            return Path.DirectorySeparatorChar + path;
        }

        /// <summary>
        /// Appends a directory separator to the end of the path if there is not one already. 
        /// Returns the directory separator if path is empty or null.
        /// </summary>
        public static string AppendSeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

            path = RemoveTrailingWhitespace(path);
            if (path.LastIndexOf(Path.DirectorySeparatorChar) == path.Length - 1) return path;
            if (path.LastIndexOf(Path.AltDirectorySeparatorChar) == path.Length - 1) return path;

            return path + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Removes the directory separator character from the 
        /// beginning of the path, if it exists.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        /// <exception cref="ArgumentException">Thrown if path is not relative.</exception>
        public static string RemoveLeadingSeparator(string path)
        {
            if (IsPathAbsolute(path)) throw new ArgumentException("Path is not relative");
            path = RemoveLeadingWhitespace(path);
            return path.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        /// Removes the directory separator character from the 
        /// end of the path, if it exists.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        public static string RemoveTrailingSeparator(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            path = RemoveTrailingWhitespace(path);
            return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        /// Removes leading and trailing separators from the path.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if path is null.</exception>
        public static string RemoveLeadingAndTrailingSeparators(string path)
        {
            return RemoveLeadingSeparator(RemoveTrailingSeparator(path));
        }

        /// <summary>
        /// Removes the directory separator from the beginning of the path 
        /// and adds one to the end of the path.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        /// <exception cref="ArgumentException">Thrown if path is absolute.</exception>
        public static string MakeRelativeDirectoryPath(string path)
        {
            if (IsPathAbsolute(path)) throw new ArgumentException("Path cannot be absolute");
            return AppendSeparator(RemoveLeadingSeparator(path));
        }

        /// <summary>
        /// Checks if the path is an absolute local path 
        /// (e.g. C:\file.txt or C:\directory\).
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        public static bool IsPathAbsolute(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            path = RemoveLeadingWhitespace(path);
            if (path.Length < 2) return false;
            if (path[0] == Path.DirectorySeparatorChar) return false;
            if (path[0] == Path.AltDirectorySeparatorChar) return false;
            return path[1] == Path.VolumeSeparatorChar;
        }

        /// <summary>
        /// Converts all alternate directory separators to normal directory separators.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if path is null.</exception>
        public static string NormalizeSeparators(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Throws a <see cref="System.IO.FileNotFoundException"/> if the file does not exist.
        /// </summary>
        /// 
        /// <param name="path">The path to the file.</param>
        /// 
        /// <exception cref="FileNotFoundException">File was not found.</exception>
        public static void EnsureFileExists(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("File not found", path);
        }

        /// <summary>
        /// Throws a <see cref="System.IO.DirectoryNotFoundException"/> if the directory does not exist.
        /// </summary>
        /// 
        /// <param name="path">The path to the directory.</param>
        /// 
        /// <exception cref="DirectoryNotFoundException">Directory was not found.</exception>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException("Directory not found: " + path);
        }
    }
}