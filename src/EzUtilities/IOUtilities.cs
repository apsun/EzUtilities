using System;
using System.IO;

namespace EzUtilities
{
    public static class IOUtilities
    {
        /// <summary>
        /// Creates a directory, doing nothing if the directory already exists. 
        /// Returns whether a new directory was successfully created.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
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
        /// Returns false if the parent directory already exists; else, true.
        /// </summary>
        /// <param name="path">The path to the file or directory.</param>
        public static bool CreateParentDirectory(string path)
        {
            string dirName = Path.GetDirectoryName(path);
            return CreateDirectory(dirName);
        }

        /// <summary>
        /// Deletes a directory and all subdirectories and files. 
        /// Returns whether the directory was deleted.
        /// </summary>
        /// <param name="path">The path of the directory to delete.</param>
        public static bool DeleteDirectory(string path)
        {
            return DeleteDirectory(path, true);
        }

        /// <summary>
        /// Deletes a directory, and if indicated, all subdirectories and files in the directory. 
        /// Returns whether the directory was deleted.
        /// </summary>
        /// <param name="path">The path of the directory to delete.</param>
        /// <param name="recursive">Whether to delete subdirectories and files in the directory.</param>
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
        /// Attempts to delete a file. 
        /// Returns whether the file was deleted.
        /// </summary>
        /// <param name="path">The path of the file to delete.</param>
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
        /// <param name="srcPath">The path of the file to move.</param>
        /// <param name="destPath">The new path of the file.</param>
        /// <param name="overwrite">Whether to overwrite existing files.</param>
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
        /// Gets a random file name without an extension.
        /// </summary>
        public static string GetRandomFileName()
        {
            return Path.ChangeExtension(Path.GetRandomFileName(), null);
        }
    }
}