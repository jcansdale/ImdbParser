using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace System.IO {
    /// <summary>Provides methods for the creation, copying, deletion, moving, and opening of a single file, and aids in the creation of <see cref="FileStream" /> <see cref="object" />s.</summary>
    interface IFile {
        /// <summary>Determines whether the specified file exists.</summary>
        /// <param name="path">The file to check.</param>
        /// <returns>True if the caller has the required permissions and path contains the name of an existing file; otherwise, false. This method also returns false if path is null, an invalid path,
        /// or a zero-length <see cref="string" />. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns false regardless of the
        /// existence of path.</returns>
        bool Exists(string path);
    }

    /// <summary>Provides methods for the creation, copying, deletion, moving, and opening of a single file, and aids in the creation of <see cref="FileStream" /> <see cref="object" />s.</summary>
    /// <remarks><see cref="File" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class FileWrapper : IFile {
        /// <summary>Determines whether the specified file exists.</summary>
        /// <param name="path">The file to check.</param>
        /// <returns>True if the caller has the required permissions and path contains the name of an existing file; otherwise, false. This method also returns false if path is null, an invalid path,
        /// or a zero-length <see cref="string" />. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns false regardless of the
        /// existence of path.</returns>
        public bool Exists(string path) => File.Exists(path);
    }
}