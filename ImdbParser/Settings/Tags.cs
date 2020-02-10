using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;

namespace ImdbParser.Settings {
    /// <summary>Tags used to store movie information.</summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    sealed class Tags {
        /// <summary>Initialize new <see cref="Tags" /> instance.</summary>
        /// <remarks>Must be public for configuration parser.</remarks>
        public Tags() => Cast = Directors = Genres = Years = Languages = Subtitles = Title = Writers = default!;

        /// <summary>Initialize new <see cref="Tags" /> instance.</summary>
        /// <param name="cast">Tag used to name movie actors.</param>
        /// <param name="directors">Tag used to name movie directors.</param>
        /// <param name="genres">Tag used to name movie genres.</param>
        /// <param name="years">Tag used to name movie years.</param>
        /// <param name="languages">Tag used to name movie languages.</param>
        /// <param name="subtitles">Tag used to name movie subtitles.</param>
        /// <param name="title">Tag used to name movie title.</param>
        /// <param name="writers">Tag used to name movie writers.</param>
        /// <exception cref="ArgumentException"><paramref name="cast" />, <paramref name="directors" />, <paramref name="genres" />, <paramref name="languages" />, <paramref name="subtitles" />,
        /// <paramref name="title" />, <paramref name="writers" />, or <paramref name="years" /> is empty <see cref="string" /> or contains only white spaces.</exception>
        internal Tags(string cast, string directors, string genres, string years, string languages, string subtitles, string title, string writers) {
            Cast = cast.ThrowIfNullOrWhiteSpace(nameof(cast));
            Directors = directors.ThrowIfNullOrWhiteSpace(nameof(directors));
            Genres = genres.ThrowIfNullOrWhiteSpace(nameof(genres));
            Years = years.ThrowIfNullOrWhiteSpace(nameof(years));
            Languages = languages.ThrowIfNullOrWhiteSpace(nameof(languages));
            Subtitles = subtitles.ThrowIfNullOrWhiteSpace(nameof(subtitles));
            Title = title.ThrowIfNullOrWhiteSpace(nameof(title));
            Writers = writers.ThrowIfNullOrWhiteSpace(nameof(writers));
        }

        /// <summary>Tag used to name movie actors.</summary>
        /// <value>Gets tag used to name movie actors.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Cast { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie directors.</summary>
        /// <value>Gets tag used to name movie directors.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Directors { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie genres.</summary>
        /// <value>Gets tag used to name movie genres.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Genres { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie languages.</summary>
        /// <value>Gets tag used to name movie languages.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Languages { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie subtitles.</summary>
        /// <value>Gets tag used to name movie subtitles.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Subtitles { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie title.</summary>
        /// <value>Gets tag used to name movie title.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Title { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie writers.</summary>
        /// <value>Gets tag used to name movie writers.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Writers { [DebuggerStepThrough] get; private set; }

        /// <summary>Tag used to name movie years.</summary>
        /// <value>Gets tag used to name movie years.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        internal string Years { [DebuggerStepThrough] get; private set; }

        /// <summary>Value used for <see cref="DebuggerDisplayAttribute" />.</summary>
        /// <value>Gets value used for <see cref="DebuggerDisplayAttribute" />.</value>
        [DebuggerBrowsable(Never)]
        string DebuggerDisplay {
            [ExcludeFromCodeCoverage]
            // ReSharper disable once UnusedMember.Local
            get => Invariant($"{nameof(Cast)}: {Cast}; {nameof(Directors)}: {Directors}; {nameof(Genres)}: {Genres}; {nameof(Languages)}: {Languages}; {nameof(Subtitles)}: {Subtitles}; ") +
                Invariant($"{nameof(Title)}: {Title}; {nameof(Writers)}: {Writers}; {nameof(Years)}: {Years}");
        }
    }
}