using System;
using System.IO;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to create and delete files and directories.
    /// </summary>
    public static class IOUtilities
    {
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
            //Same as Directory.GetParent without the overhead
            string parentDir = Path.GetDirectoryName(Path.GetFullPath(path));

            //Will never be null, Path.GetFullPath throws the same exception anyways
            if (parentDir == null) throw new ArgumentNullException("path");

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
        /// Gets a random file name without an extension.
        /// </summary>
        public static string GetRandomFileName()
        {
            return Path.ChangeExtension(Path.GetRandomFileName(), null);
        }

        /// <summary>
        /// Appends a directory seperator to the end of the path if there is not one already.
        /// </summary>
        public static string AppendSeperator(string path)
        {
            path = path.TrimEnd(' ', '\t', '\n', '\v', '\f', '\r', '\x0085');
            if (path.LastIndexOf(Path.DirectorySeparatorChar) == path.Length - 1) return path;
            if (path.LastIndexOf(Path.AltDirectorySeparatorChar) == path.Length - 1) return path;

            return path + Path.DirectorySeparatorChar;
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