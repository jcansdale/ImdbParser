using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;
using static System.Result;
using static System.String;

namespace ImdbParser {
    /// <summary>Information about movie.</summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    sealed class Movie : IDisposable {
        /// <summary>Movie's cover.</summary>
        IImage? _cover;

        /// <summary>Initialize new <see cref="Movie" /> instance.</summary>
        /// <param name="title">Title of the movie.</param>
        /// <param name="years">Years when the movie was released.</param>
        /// <param name="genres">Movie's genres.</param>
        /// <param name="cover">Movie's cover.</param>
        /// <param name="directors">Movie's directors.</param>
        /// <param name="writers">Movie's writers.</param>
        /// <param name="cast">Actors who cast in the movie.</param>
        Movie(string title, int years, IEnumerable<string> genres, IImage cover, IEnumerable<string> directors, IEnumerable<string> writers, IEnumerable<string> cast) {
            Title = title;
            Years = years;
            Genres = genres;
            _cover = cover;
            Directors = directors;
            Writers = writers;
            Cast = cast;
        }

        /// <summary>Actors who cast in the movie.</summary>
        /// <value>Gets actors who cast in the movie.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal IEnumerable<string> Cast { [DebuggerStepThrough] get; }

        /// <summary>Movie's directors.</summary>
        /// <value>Gets movie's directors.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal IEnumerable<string> Directors { [DebuggerStepThrough] get; }

        /// <summary>Movie's genres.</summary>
        /// <value>Gets movie's genres.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal IEnumerable<string> Genres { [DebuggerStepThrough] get; }

        /// <summary>Years when the movie was released.</summary>
        /// <value>Gets years when the movie was released.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal int Years { [DebuggerStepThrough] get; }

        /// <summary>Title of the movie.</summary>
        /// <value>Gets title of the movie.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal string Title { [DebuggerStepThrough] get; }

        /// <summary>Movie's writers.</summary>
        /// <value>Gets movie's writers.</value>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal IEnumerable<string> Writers { [DebuggerStepThrough] get; }

        /// <summary>Value used for <see cref="DebuggerDisplayAttribute" />.</summary>
        /// <value>Gets value used for <see cref="DebuggerDisplayAttribute" />.</value>
        [DebuggerBrowsable(Never)]
        string DebuggerDisplay {
            [ExcludeFromCodeCoverage]
            // ReSharper disable once UnusedMember.Local
            get => Invariant($"{nameof(Cast)}: {Cast.Join(", ")}; {nameof(Directors)}: {Directors.Join(", ")}; {nameof(Genres)}: {Genres.Join(", ")}; ") +
                Invariant($"{nameof(Title)}: {Title}; {nameof(Writers)}: {Writers.Join(", ")}; {nameof(Years)}: {Years}");
        }

        /// <summary>Creates information about the movie.</summary>
        /// <param name="title">Title of the movie.</param>
        /// <param name="years">Years when the movie was released.</param>
        /// <param name="genres">Movie's genres.</param>
        /// <param name="cover">Movie's cover.</param>
        /// <param name="directors">Movie's directors.</param>
        /// <param name="writers">Movie's writers.</param>
        /// <param name="cast">Actors who cast in the movie.</param>
        /// <returns>Created movie's information.</returns>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller should dispose the object.")]
        internal static DisposableResult<Movie>
            CreateMovie(string title, int years, IReadOnlyCollection<string> genres, IImage cover, IReadOnlyCollection<string> directors, IEnumerable<string> writers,
                IReadOnlyCollection<string> cast) => IsNullOrWhiteSpace(title) ? DisposableFail<Movie>(Failed_to_get_title) : years <= 0 ? DisposableFail<Movie>(Failed_to_get_years) : !genres.Any() ?
            DisposableFail<Movie>(Failed_to_get_genres) : !directors.Any() ? DisposableFail<Movie>(Failed_to_get_directors) : !cast.Any() ? DisposableFail<Movie>(Failed_to_get_cast) :
                DisposableOk(new Movie(title, years, genres, cover, directors, writers, cast));

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (_cover == default) return;
            _cover.Dispose();
            _cover = default;
        }

        /// <summary>Gets movie's genres.</summary>
        /// <returns>Movie's genres.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        internal IImage Cover() => _cover ?? throw new ObjectDisposedException(nameof(_cover));
    }
}