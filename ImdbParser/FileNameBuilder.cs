using ImdbParser.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;
using static System.IO.Path;
using static System.String;

namespace ImdbParser {
    /// <summary>Builds files names.</summary>
    interface IFileNameBuilder {
        /// <summary>Builds file name using <paramref name="movie" />s information, <paramref name="languages" />, and <paramref name="subtitles" />.</summary>
        /// <param name="movie">Movie which information should be used.</param>
        /// <param name="languages">Soundtrack languages.</param>
        /// <param name="subtitles">Subtitles languages.</param>
        /// <returns>File name that should be used for <paramref name="movie" />.</returns>
        FileInfo BuildFileName(Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles);
    }

    /// <summary>Builds files names.</summary>
    sealed class FileNameBuilder : IFileNameBuilder {
        /// <summary>Separator used to separate multiple values.</summary>
        [DebuggerBrowsable(Never)] const string MultipleValuesSeparator = ", ";
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;

        /// <summary>Initialize new <see cref="FileNameBuilder" /> instance.</summary>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public FileNameBuilder(ISettings settings) => _settings = settings;

        /// <summary>Formats <paramref name="languages" /> for file name.</summary>
        /// <param name="languages">Languages to format.</param>
        /// <returns><paramref name="languages" /> formatted for file name.</returns>
        static string ManageLanguages(IEnumerable<Language> languages) => languages.Select(language => language.ToString().ToUpperInvariant()).Join(MultipleValuesSeparator);

        /// <summary>Builds file name using <paramref name="movie" />s information, <paramref name="languages" />, and <paramref name="subtitles" />.</summary>
        /// <param name="movie">Movie which information should be used.</param>
        /// <param name="languages">Soundtrack languages.</param>
        /// <param name="subtitles">Subtitles languages.</param>
        /// <returns>File name that should be used for <paramref name="movie" />.</returns>
        public FileInfo BuildFileName(Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles) => new FileInfo(Combine(_settings.MoviesDirectory.FullName,
            movie.Title.Where(character => !GetInvalidFileNameChars().ToHashSet().Contains(character)).Where(character => !GetInvalidPathChars().ToHashSet().Contains(character)).Join() +
            Invariant($" ({movie.Years}); {movie.Genres.Join(MultipleValuesSeparator)}; {ManageLanguages(languages)}") +
            Invariant($"{(subtitles.Any() ? Invariant($"; {ManageLanguages(subtitles)}") : Empty)}.mkv")));
    }
}