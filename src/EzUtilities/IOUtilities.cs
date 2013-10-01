using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to work with directories, files, and paths.
    /// </summary>
    public static class IOUtilities
    {
        /// <summary>
        /// Creates a directory, doing nothing if the directory already exists. 
        /// This does not throw an exception if the creation fails.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <returns>
        /// False if the creation failed or the directory already exists; true otherwise.
        /// </returns>
        public static bool TryCreateDirectory(string path)
        {
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
        /// Deletes a directory and all subdirectories and files, 
        /// without throwing an exception if the operation fails.
        /// </summary>
        /// <param name="path">The path of the directory to delete.</param>
        /// <returns>True if the directory was successfully deleted; false otherwise.</returns>
        public static bool TryDeleteDirectory(string path)
        {
            return TryDeleteDirectory(path, true);
        }

        /// <summary>
        /// Deletes a directory, and if indicated, all subdirectories 
        /// and files in the directory, without throwing an 
        /// exception if the operation fails.
        /// </summary>
        /// <param name="path">The path of the directory to delete.</param>
        /// <param name="recursive">Whether to delete subdirectories and files.</param>
        /// <returns>True if the directory was successfully deleted; false otherwise.</returns>
        public static bool TryDeleteDirectory(string path, bool recursive)
        {
            if (!Directory.Exists(path)) return false;

            try
            {
                Directory.Delete(path, recursive);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the specified file, without throwing an 
        /// exception if the operation fails.
        /// </summary>
        /// <param name="path">The path of the file to delete.</param>
        /// <returns>True if the file was successfully deleted; false otherwise.</returns>
        public static bool TryDeleteFile(string path)
        {
            if (!File.Exists(path)) return false;

            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes multiple files, without throwing an exception if 
        /// any files could not be deleted.
        /// </summary>
        /// <param name="paths">The paths of the files to delete.</param>
        /// <returns>True if all files were successfully deleted; false otherwise.</returns>
        public static bool TryDeleteFiles(IEnumerable<string> paths)
        {
            return paths.Aggregate(true, (current, file) => TryDeleteFile(file) && current);
        }

        /// <summary>
        /// Deletes multiple directories and all subdirectories 
        /// and files within the directories, without throwing an exception if 
        /// any directories or files could not be deleted.
        /// </summary>
        /// <param name="paths">The paths of the directories to delete.</param>
        /// <returns>True if all directories were successfully deleted; false otherwise.</returns>
        public static bool TryDeleteDirectories(IEnumerable<string> paths)
        {
            return TryDeleteDirectories(paths, true);
        }

        /// <summary>
        /// Deletes multiple directories, and, if indicated, 
        /// all subdirectories and files within the directories, 
        /// without throwing an exception if any directories or 
        /// files could not be deleted.
        /// </summary>
        /// <param name="paths">The paths of the directories to delete.</param>
        /// <param name="recursive">Whether to delete sub-directories and files.</param>
        /// <returns>True if all directories were successfully deleted; false otherwise.</returns>
        public static bool TryDeleteDirectories(IEnumerable<string> paths, bool recursive)
        {
            return paths.Aggregate(true, (current, dir) => TryDeleteDirectory(dir, recursive) && current);
        }

        /// <summary>
        /// Deletes all files and directories in the specified directory. 
        /// Returns false if the path is not a directory, the directory 
        /// does not exist, or if any items were not successfully deleted.
        /// </summary>
        /// <param name="path">The path of the directory to empty.</param>
        /// <returns>Whether all items within the directory were deleted.</returns>
        /// TODO: ADD EXCEPTION DOC
        public static bool TryEmptyDirectory(string path)
        {
            if (!Directory.Exists(path)) return false;

            return TryDeleteDirectories(Directory.GetDirectories(path)) &
                   TryDeleteFiles(Directory.GetFiles(path));
        }

        /// <summary>
        /// Gets a random 8 character file name without an extension.
        /// </summary>
        public static string GetRandomFileName()
        {
            return Path.ChangeExtension(Path.GetRandomFileName(), null);
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
        /// <param name="path">The path to the file.</param>
        /// <exception cref="FileNotFoundException">Thrown if the file was not found.</exception>
        public static void EnsureFileExists(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("File not found", path);
        }

        /// <summary>
        /// Throws a <see cref="System.IO.DirectoryNotFoundException"/> if the directory does not exist.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        /// <exception cref="DirectoryNotFoundException">Thrown if the directory was not found.</exception>
        public static void EnsureDirectoryExists(string path)
        {
            EnsureDirectoryExists(path, false);
        }

        /// <summary>
        /// Ensures that the directory exists, either by creating it or by throwing an exception.
        /// </summary>
        /// <param name="path">
        /// The path to the directory.
        /// </param>
        /// <param name="create">
        /// True to create the directory if it does not exist; false to throw an exception.
        /// </param>
        /// <exception cref="DirectoryNotFoundException">
        /// Thrown if the directory was not found and create is false.
        /// </exception>
        /// TODO: ADD EXCEPTION DOC
        public static void EnsureDirectoryExists(string path, bool create)
        {
            if (Directory.Exists(path)) return;
            if (create)
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                throw new DirectoryNotFoundException("Directory not found: " + path);
            }
        }

        /// <summary>
        /// Creates the directory that contains this file or directory. 
        /// </summary>
        /// <param name="path">The path to the file or directory.</param>
        /// <returns>False if the parent directory already exists; true otherwise.</returns>
        /// TODO: ADD EXCEPTION DOC
        public static bool CreateParentDirectory(string path)
        {
            string parentDir = GetParentDirectory(path);
            if (Directory.Exists(parentDir)) return false;
            Directory.CreateDirectory(parentDir);
            return true;
        }

        /// <summary>
        /// Moves a file, overwriting the destination file if it exists.
        /// </summary>
        /// <param name="srcPath">The path of the file to move.</param>
        /// <param name="destPath">The new path of the file.</param>
        /// <returns>True if the destination file existed; false otherwise.</returns>
        /// TODO: ADD EXCEPTION DOC
        public static bool MoveReplaceFile(string srcPath, string destPath)
        {
            if (srcPath == null) throw new ArgumentNullException("srcPath");
            if (destPath == null) throw new ArgumentNullException("destPath");
            bool deletedDest;
            if (File.Exists(destPath))
            {
                deletedDest = true;
                File.Delete(destPath);
            }
            else
            {
                deletedDest = false;
            }
            
            File.Move(srcPath, destPath);
            return deletedDest;
        }

        /// <summary>
        /// Gets the parent directory of the specified file or directory.
        /// </summary>
        /// <param name="path">The path of the file or directory.</param>
        /// <returns>The path of the parent directory.</returns>
        /// TODO: ADD EXCEPTION DOC
        public static string GetParentDirectory(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            //Even if the path is not a directory, 
            //we can pretend it is; after all, folders 
            //can be named folder.exe as well.
            var parentDi = new DirectoryInfo(path).Parent;
            if (parentDi == null) throw new ArgumentException("Path is a root path");
            string parent = parentDi.FullName;
            return parent;
        }

        /// <summary>
        /// Gets the path of a directory or file relative to a base directory.
        /// </summary>
        /// <param name="baseDir">The path of the base directory.</param>
        /// <param name="fullPath">The path of the file or directory.</param>
        /// <returns>A string containing the path of fullPath relative to baseDir.</returns>
        /// TODO: ADD EXCEPTION DOC
        public static string GetRelativePath(string baseDir, string fullPath)
        {
            if (baseDir == null) throw new ArgumentNullException("baseDir");
            if (fullPath == null) throw new ArgumentNullException("fullPath");

            string baseDirNormalized = Path.GetFullPath(baseDir + Path.DirectorySeparatorChar);
            string baseDirNormalizedUpper = baseDirNormalized.ToUpperInvariant();
            string fullPathNormalized = Path.GetFullPath(fullPath);
            string fullPathNormalizedUpper = fullPathNormalized.ToUpperInvariant();
            if (fullPathNormalizedUpper.IndexOf(baseDirNormalizedUpper, StringComparison.Ordinal) != 0)
            {
                throw new ArgumentException("The path is not located within the base directory");
            }

            return fullPathNormalized.Remove(0, baseDirNormalized.Length);
        }
    }
}