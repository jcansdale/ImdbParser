using System.Diagnostics;
using System.Security;

// ReSharper disable once CheckNamespace
namespace System.IO {
    /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="FileStream" />
    /// <see cref="object" />s.</summary>
    interface IFileInfo : IFileSystemInfo {
        /// <summary>Permanently deletes a file.</summary>
        /// <exception cref="IOException">The target file is open or memory-mapped on a computer running Microsoft Windows NT. -or- There is an open handle on the file, and the operating system is
        /// Windows XP or earlier. This open handle can result from enumerating directories and files.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The path is a directory.</exception>
        void Delete();
    }

    /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="FileStream" />
    /// <see cref="object" />s.</summary>
    /// <remarks><see cref="FileInfo" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class FileInfoWrapper : FileSystemInfoWrapper, IFileInfo {
        /// <summary><see cref="FileInfo" /> to wrap.</summary>
        readonly FileInfo _info;

        /// <summary>Initialize new <see cref="FileInfoWrapper" /> instance.</summary>
        /// <param name="info"><see cref="FileInfo" /> to wrap.</param>
        internal FileInfoWrapper(FileInfo info) : base(info) => _info = info;

        /// <summary>Permanently deletes a file.</summary>
        /// <exception cref="IOException">The target file is open or memory-mapped on a computer running Microsoft Windows NT. -or- There is an open handle on the file, and the operating system is
        /// Windows XP or earlier. This open handle can result from enumerating directories and files.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The path is a directory.</exception>
        public void Delete() => _info.Delete();
    }
}