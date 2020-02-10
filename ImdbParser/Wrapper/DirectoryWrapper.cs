using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace System.IO {
    /// <summary>Exposes methods for creating, moving, and enumerating through directories and subdirectories.</summary>
    interface IDirectory {
        /// <summary>Creates all directories and subdirectories in the specified <paramref name="path" /> unless they already exist.</summary>
        /// <param name="path">The directory to create.</param>
        /// <returns>An <see cref="object" /> that represents the directory at the specified <paramref name="path" />. This <see cref="object" /> is returned regardless of whether a directory at the
        /// specified path already exists.</returns>
        /// <exception cref="IOException">The directory specified by <paramref name="path" /> is a file. -or- The network name is not known.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is a zero-length <see cref="string" />, contains only white space, or contains one or more invalid characters. You can query
        /// for invalid characters by using the <see cref="Path.GetInvalidPathChars" /> method. -or- <paramref name="path" /> is prefixed with, or contains, only a colon character (:).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is null.</exception>
        /// <exception cref="PathTooLongException">The specified <paramref name="path" />, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified <paramref name="path" />, is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        IDirectoryInfo CreateDirectory(string path);
    }

    /// <summary>Exposes methods for creating, moving, and enumerating through directories and subdirectories.</summary>
    /// <remarks><see cref="Directory" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class DirectoryWrapper : IDirectory {
        /// <summary>Creates all directories and subdirectories in the specified <paramref name="path" /> unless they already exist.</summary>
        /// <param name="path">The directory to create.</param>
        /// <returns>An <see cref="object" /> that represents the directory at the specified <paramref name="path" />. This <see cref="object" /> is returned regardless of whether a directory at the
        /// specified path already exists.</returns>
        /// <exception cref="IOException">The directory specified by <paramref name="path" /> is a file. -or- The network name is not known.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is a zero-length <see cref="string" />, contains only white space, or contains one or more invalid characters. You can query
        /// for invalid characters by using the <see cref="Path.GetInvalidPathChars" /> method. -or- <paramref name="path" /> is prefixed with, or contains, only a colon character (:).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is null.</exception>
        /// <exception cref="PathTooLongException">The specified <paramref name="path" />, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified <paramref name="path" />, is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        public IDirectoryInfo CreateDirectory(string path) => new DirectoryInfoWrapper(Directory.CreateDirectory(path));
    }
}