using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.IO.Path;

namespace ImdbParser.Settings {
    /// <summary>Application settings.</summary>
    interface ISettings {
        /// <summary>Name for cover saving.</summary>
        /// <value>Gets name for cover saving.</value>
        FileInfo CoverName { get; }

        /// <summary>Names of folders in the library where shortcuts should be created.</summary>
        /// <value>Gets names of folders in the library where shortcuts should be created.</value>
        LibraryFolders Folders { get; }

        /// <summary>Directory where libraries are stored.</summary>
        /// <value>Gets directory where libraries are stored.</value>
        IDirectoryInfo LibraryDirectory { get; }

        /// <summary>Directory where movies are stored.</summary>
        /// <value>Gets directory where movies are stored</value>
        DirectoryInfo MoviesDirectory { get; }

        /// <summary>Tags used to store movie information.</summary>
        /// <value>Gets tags used to store movie information.</value>
        Tags Tags { get; }

        /// <summary>Name for <see cref="Tags" /> saving.</summary>
        /// <value>Gets name for <see cref="Tags" /> saving.</value>
        FileInfo TagsName { get; }
    }

    /// <summary>Application settings.</summary>
    class ApplicationSettings : ISettings {
        /// <summary>File where application settings are stored.</summary>
        [DebuggerBrowsable(Never)] const string ApplicationSettingsFileName = "AppSettings.json";
        /// <summary>File where local application settings are stored.</summary>
        [DebuggerBrowsable(Never)] const string ApplicationSettingsLocalFileName = "AppSettings.Local.json";
        /// <summary>Name for cover saving.</summary>
        [DebuggerBrowsable(Never)] readonly Lazy<FileInfo> _coverName;
        /// <summary>Names of folders in the library where shortcuts should be created.</summary>
        [DebuggerBrowsable(Never)] readonly Lazy<LibraryFolders> _folders;
        /// <summary>Directory where libraries are stored.</summary>
        [DebuggerBrowsable(Never)] readonly Lazy<IDirectoryInfo> _libraryDirectory;
        /// <summary>Directory where movies are stored.</summary>
        [DebuggerBrowsable(Never)] readonly Lazy<DirectoryInfo> _moviesDirectory;
        /// <summary>Tags used to store movie information.</summary>
        [DebuggerBrowsable(Never)] readonly Lazy<Tags> _tags;
        /// <summary>Name for <see cref="Tags" /> saving.</summary>
        [DebuggerBrowsable(Never)] readonly Lazy<FileInfo> _tagsName;

        /// <summary>Initialize new <see cref="ApplicationSettings" /> instance.</summary>
        /// <remarks>Must be public for dependency injection.</remarks>
        public ApplicationSettings() : this(new ConfigurationBuilder().AddJsonFile(ApplicationSettingsFileName).AddJsonFile(ApplicationSettingsLocalFileName, true).Build()) {}

        /// <summary>Initialize new <see cref="ApplicationSettings" /> instance.</summary>
        /// <param name="root">Configuration to use.</param>
        internal ApplicationSettings(IConfiguration root) {
            _coverName = new Lazy<FileInfo>(() => new FileInfo(Combine(LibraryDirectory.FullName, root[nameof(CoverName)])));
            _folders = new Lazy<LibraryFolders>(() => root.GetSection(nameof(Folders)).Get<LibraryFolders>(options => options.BindNonPublicProperties = true));
            _libraryDirectory = new Lazy<IDirectoryInfo>(() => new DirectoryInfoWrapper(new DirectoryInfo(Combine(MoviesDirectory.FullName, root[nameof(LibraryDirectory)]))));
            _moviesDirectory = new Lazy<DirectoryInfo>(() => new DirectoryInfo(root[nameof(MoviesDirectory)]));
            _tags = new Lazy<Tags>(() => root.GetSection(nameof(Tags)).Get<Tags>(options => options.BindNonPublicProperties = true));
            _tagsName = new Lazy<FileInfo>(() => new FileInfo(Combine(LibraryDirectory.FullName, root[nameof(TagsName)])));
        }

        /// <summary>Name for cover saving.</summary>
        /// <value>Gets name for cover saving.</value>
        public FileInfo CoverName { [DebuggerStepThrough] get => _coverName.Value; }

        /// <summary>Names of folders in the library where shortcuts should be created.</summary>
        /// <value>Gets names of folders in the library where shortcuts should be created.</value>
        public LibraryFolders Folders { [DebuggerStepThrough] get => _folders.Value; }

        /// <summary>Directory where libraries are stored.</summary>
        /// <value>Gets directory where libraries are stored.</value>
        public IDirectoryInfo LibraryDirectory { [DebuggerStepThrough] get => _libraryDirectory.Value; }

        /// <summary>Directory where movies are stored.</summary>
        /// <value>Gets directory where movies are stored</value>
        public DirectoryInfo MoviesDirectory { [DebuggerStepThrough] get => _moviesDirectory.Value; }

        /// <summary>Tags used to store movie information.</summary>
        /// <value>Gets tags used to store movie information.</value>
        public Tags Tags { [DebuggerStepThrough] get => _tags.Value; }

        /// <summary>Name for <see cref="Tags" /> saving.</summary>
        /// <value>Gets name for <see cref="Tags" /> saving.</value>
        public FileInfo TagsName { [DebuggerStepThrough] get => _tagsName.Value; }
    }
}