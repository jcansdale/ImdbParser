using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

// ReSharper disable once CheckNamespace
namespace System.IO {
    /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories.</summary>
    interface IDirectoryInfo : IFileSystemInfo {
        /// <summary>Deletes this <see cref="IDirectoryInfo" /> if it is empty.</summary>
        /// <exception cref="UnauthorizedAccessException">The directory contains a read-only file.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory described by this <see cref="IDirectoryInfo" /> <see cref="object" /> does not exist or could not be found.</exception>
        /// <exception cref="IOException">The directory is not empty. -or- The directory is the application's current working directory. -or- There is an open handle on the directory, and the
        /// operating system is Windows XP or earlier. This open handle can result from enumerating directories.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        void Delete();

        // ReSharper disable once CommentTypo
        /// <param name="searchPattern">The search <see cref="string" /> to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (* and
        /// ?) characters, but it doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is
        /// <see cref="SearchOption.TopDirectoryOnly" />.</param>
        /// <returns>AnAn enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="searchPattern" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption" /> is not a valid <see cref="SearchOption" /> value.</exception>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in the <see cref="DirectoryInfoWrapper" /> <see cref="object" /> is invalid (for example, it is on an unmapped
        /// drive).</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption);

        /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
        /// <param name="searchPattern">The search <see cref="string" /> to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?)
        /// characters, but it doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="searchPattern" /> is null.</exception>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in the <see cref="DirectoryInfo" /> <see cref="object" /> is invalid, (for example, it is on an unmapped
        /// drive).</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        IEnumerable<IFileInfo> EnumerateFiles(string searchPattern);

        /// <summary>Returns an enumerable collection of file system information in the current directory.</summary>
        /// <returns>An enumerable collection of file system information in the current directory.</returns>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in the <see cref="IDirectoryInfo" /> <see cref="object" /> is invalid (for example, it is on an unmapped
        /// drive).</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();

        /// <summary>Returns the original path that was passed to the <see cref="DirectoryInfoWrapper" /> constructor.</summary>
        /// <returns>The original path that was passed by the user.</returns>
        string ToString();
    }

    /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories.</summary>
    /// <remarks><see cref="DirectoryInfo" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class DirectoryInfoWrapper : FileSystemInfoWrapper, IDirectoryInfo {
        /// <summary><see cref="DirectoryInfo" /> to wrap.</summary>
        readonly DirectoryInfo _info;

        /// <summary>Initialize new <see cref="DirectoryInfoWrapper" /> instance.</summary>
        /// <param name="info"><see cref="DirectoryInfo" /> to wrap.</param>
        internal DirectoryInfoWrapper(DirectoryInfo info) : base(info) => _info = info;

        /// <summary>Deletes this <see cref="IDirectoryInfo" /> if it is empty.</summary>
        /// <exception cref="UnauthorizedAccessException">The directory contains a read-only file.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory described by this <see cref="IDirectoryInfo" /> <see cref="object" /> does not exist or could not be found.</exception>
        /// <exception cref="IOException">The directory is not empty. -or- The directory is the application's current working directory. -or- There is an open handle on the directory, and the
        /// operating system is Windows XP or earlier. This open handle can result from enumerating directories.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        public void Delete() => _info.Delete();

        // ReSharper disable once CommentTypo
        /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
        /// <param name="searchPattern">The search <see cref="string" /> to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (* and
        /// ?) characters, but it doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is
        /// <see cref="SearchOption.TopDirectoryOnly" />.</param>
        /// <returns>AnAn enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="searchPattern" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption" /> is not a valid <see cref="SearchOption" /> value.</exception>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in the <see cref="DirectoryInfoWrapper" /> <see cref="object" /> is invalid (for example, it is on an unmapped
        /// drive).</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption) =>
            _info.EnumerateDirectories(searchPattern, searchOption).Select(directory => new DirectoryInfoWrapper(directory));

        /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
        /// <param name="searchPattern">The search <see cref="string" /> to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?)
        /// characters, but it doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="searchPattern" /> is null.</exception>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in the <see cref="DirectoryInfo" /> <see cref="object" /> is invalid, (for example, it is on an unmapped
        /// drive).</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern) => _info.EnumerateFiles(searchPattern).Select(file => new FileInfoWrapper(file));

        /// <summary>Returns an enumerable collection of file system information in the current directory.</summary>
        /// <returns>An enumerable collection of file system information in the current directory.</returns>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in the <see cref="IDirectoryInfo" /> <see cref="object" /> is invalid (for example, it is on an unmapped
        /// drive).</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos() => _info.EnumerateFileSystemInfos().
            Select(info => info is DirectoryInfo directory ? (IFileSystemInfo)new DirectoryInfoWrapper(directory) : new FileInfoWrapper((FileInfo)info));

        /// <summary>Returns the original path that was passed to the <see cref="DirectoryInfoWrapper" /> constructor.</summary>
        /// <returns>The original path that was passed by the user.</returns>
        public override string ToString() => _info.ToString();
    }
}