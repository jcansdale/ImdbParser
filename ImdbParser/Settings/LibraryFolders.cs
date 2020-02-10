using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;

namespace ImdbParser.Settings {
    /// <summary>Names of folders in the library.</summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    sealed class LibraryFolders {
        /// <summary>Initialize new <see cref="LibraryFolders" /> instance.</summary>
        /// <remarks>Must be public for configuration parser.</remarks>
        public LibraryFolders() => Cast = Directors = Genres = Years = default!;

        /// <summary>Initialize new <see cref="LibraryFolders" /> instance.</summary>
        /// <param name="cast">Folder where movie actor's shortcuts should be placed.</param>
        /// <param name="directors">Folder where movie director's shortcuts should be placed.</param>
        /// <param name="genres">Folder where movie genre's shortcuts should be placed.</param>
        /// <param name="years">Folder where movie year's shortcut should be placed.</param>
        /// <exception cref="ArgumentException"><paramref name="cast" />, <paramref name="directors" />, <paramref name="genres" />, or <paramref name="years" /> is empty <see cref="string" /> or
        /// contains only white spaces.</exception>
        internal LibraryFolders(string cast, string directors, string genres, string years) {
            Cast = cast.ThrowIfNullOrWhiteSpace(nameof(cast));
            Directors = directors.ThrowIfNullOrWhiteSpace(nameof(directors));
            Genres = genres.ThrowIfNullOrWhiteSpace(nameof(genres));
            Years = years.ThrowIfNullOrWhiteSpace(nameof(years));
        }

        /// <summary>Folder where movie actor's shortcuts should be placed.</summary>
        /// <value>Gets folder where movie actor's shortcuts should be placed.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Cast { [DebuggerStepThrough] get; private set; }

        /// <summary>Folder where movie director's shortcuts should be placed.</summary>
        /// <value>Gets folder where movie director's shortcuts should be placed.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Directors { [DebuggerStepThrough] get; private set; }

        /// <summary>Folder where movie genre's shortcuts should be placed.</summary>
        /// <value>Gets folder where movie genre's shortcuts should be placed.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Genres { [DebuggerStepThrough] get; private set; }

        /// <summary>Folder where movie year's shortcut should be placed.</summary>
        /// <value>Gets folder where movie year's shortcut should be placed.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Years { [DebuggerStepThrough] get; private set; }

        /// <summary>Value used for <see cref="DebuggerDisplayAttribute" />.</summary>
        /// <value>Gets value used for <see cref="DebuggerDisplayAttribute" />.</value>
        [DebuggerBrowsable(Never)]
        // ReSharper disable once UnusedMember.Local
        string DebuggerDisplay { [ExcludeFromCodeCoverage] get => Invariant($"{nameof(Cast)}: {Cast}; {nameof(Directors)}: {Directors}; {nameof(Genres)}: {Genres}; {nameof(Years)}: {Years}"); }
    }
}