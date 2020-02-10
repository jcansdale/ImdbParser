using System.Diagnostics;
using System.Security;

// ReSharper disable once CheckNamespace
namespace System.IO {
    /// <summary>Provides the base interface for both <see cref="IFileInfo" /> and <see cref="IDirectoryInfo" /> <see cref="object" />s.</summary>
    interface IFileSystemInfo {
        /// <summary>Gets the full path of the directory or file.</summary>
        /// <returns>A <see cref="string" /> containing the full path.</returns>
        /// <exception cref="PathTooLongException">The fully qualified path and file name exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        string FullName { get; }
    }

    /// <summary>Provides the base class for both <see cref="FileInfoWrapper" /> and <see cref="DirectoryInfoWrapper" /> <see cref="object" />s.</summary>
    /// <remarks><see cref="FileSystemInfo" /> wrapper.</remarks>
    [DebuggerStepThrough]
    abstract class FileSystemInfoWrapper : IFileSystemInfo {
        /// <summary><see cref="FileSystemInfo" /> to wrap.</summary>
        readonly FileSystemInfo _info;

        /// <summary>Initialize new <see cref="FileSystemInfoWrapper" /> instance.</summary>
        /// <param name="info"><see cref="FileSystemInfo" /> to wrap.</param>
        private protected FileSystemInfoWrapper(FileSystemInfo info) => _info = info;

        /// <summary>Gets the full path of the directory or file.</summary>
        /// <returns>A <see cref="string" /> containing the full path.</returns>
        /// <exception cref="PathTooLongException">The fully qualified path and file name exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        public string FullName => _info.FullName;
    }
}