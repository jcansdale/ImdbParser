using ImdbParser.Settings;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Globalization.CultureInfo;
using static System.IO.Path;
using static System.Threading.Tasks.Parallel;

namespace ImdbParser {
    /// <summary>Creates shortcuts to the movie.</summary>
    interface IShortcutCreator {
        /// <summary>Creates shortcut for <paramref name="movie" />.</summary>
        /// <param name="directory">Directory, where shortcut should be created.</param>
        /// <param name="movie"><see cref="Movie" /> for which shortcut should be created.</param>
        void CreateShortcut(IDirectoryInfo directory, FileInfo movie);

        /// <summary>Creates shortcuts for <paramref name="movie" />.</summary>
        /// <param name="movie"><see cref="Movie" /> for which shortcuts should be created.</param>
        /// <param name="languages">Soundtrack languages.</param>
        /// <param name="subtitles">Subtitles languages.</param>
        void CreateShortcuts(Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles);
    }

    /// <summary>Creates shortcuts to the movie.</summary>
    sealed class ShortcutCreator : IShortcutCreator {
        /// <summary>Extension used for links.</summary>
        [DebuggerBrowsable(Never)] const string LinkFileExtension = ".lnk";
        /// <summary>Identifier for link icon location.</summary>
        [DebuggerBrowsable(Never)] const string LinkIconLocation = ", 0";
        /// <summary>File name builder.</summary>
        readonly IFileNameBuilder _builder;
        /// <summary>Directories creator.</summary>
        readonly IDirectory _directory;
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;
        /// <summary>Shell to use.</summary>
        readonly IWshShell3 _shell;

        /// <summary>Initialize new <see cref="ShortcutCreator" /> instance.</summary>
        /// <param name="builder">File name builder.</param>
        /// <param name="shell">Shell to use.</param>
        /// <param name="directory">Directories creator.</param>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public ShortcutCreator(IFileNameBuilder builder, IWshShell3 shell, IDirectory directory, ISettings settings) {
            _builder = builder;
            _shell = shell;
            _directory = directory;
            _settings = settings;
        }

        /// <summary>Creates shortcut for <paramref name="movie" />.</summary>
        /// <param name="directory">Directory, where shortcut should be created.</param>
        /// <param name="movie"><see cref="Movie" /> for which shortcut should be created.</param>
        public void CreateShortcut(IDirectoryInfo directory, FileInfo movie) => CreateShortcut(directory.ToString(), movie);

        /// <summary>Creates shortcuts for <paramref name="movie" />.</summary>
        /// <param name="movie"><see cref="Movie" /> for which shortcuts should be created.</param>
        /// <param name="languages">Soundtrack languages.</param>
        /// <param name="subtitles">Subtitles languages.</param>
        public void CreateShortcuts(Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles) => _builder.BuildFileName(movie, languages, subtitles).
            ParallelDo(name => CreateDirectories(_settings.Folders.Cast, movie.Cast, name), name => CreateDirectories(_settings.Folders.Directors, movie.Directors, name),
                name => CreateDirectories(_settings.Folders.Genres, movie.Genres, name), name => CreateDirectories(_settings.Folders.Years, new[] { movie.Years.ToString(InvariantCulture) }, name));

        /// <summary>Creates directories with shortcuts in <paramref name="directoryName" />.</summary>
        /// <param name="directoryName">Base directory name, in which inner directories should be created.</param>
        /// <param name="innerDirectories">Directories, which should be created inside <paramref name="directoryName" />, names.</param>
        /// <param name="shortcutTarget">Path, which should be targeted by shortcuts.</param>
        void CreateDirectories(string directoryName, IEnumerable<string> innerDirectories, FileSystemInfo shortcutTarget) => Combine(_settings.LibraryDirectory.ToString(), directoryName).Do(library =>
            ForEach(innerDirectories,
                innerDirectory => Combine(library, innerDirectory).Do(fullPath => _directory.CreateDirectory(fullPath)).Do(fullPath => CreateShortcut(fullPath, shortcutTarget))));

        /// <summary>Creates shortcut to <paramref name="shortcutTarget" /> in <paramref name="directoryName" />.</summary>
        /// <param name="directoryName">Directory path where shortcut should be created.</param>
        /// <param name="shortcutTarget">Path, which should be targeted by shortcuts.</param>
        void CreateShortcut(string directoryName, FileSystemInfo shortcutTarget) {
            var link = (IWshShortcut)_shell.CreateShortcut(Combine(directoryName, shortcutTarget.Name) + LinkFileExtension);
            link.TargetPath = shortcutTarget.ToString();
            link.IconLocation = shortcutTarget + LinkIconLocation;
            link.Save();
        }
    }
}