using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.FormattableString;
using static System.Result;
using static System.Text.RegularExpressions.Regex;
using static System.UriKind;

namespace ImdbParser {
    /// <summary>Provides an <see cref="object" /> representation of a uniform resource identifier (URI) for IMDB page.</summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    sealed class ImdbFilmUrl : Uri {
        /// <summary>Page on which cast and crew information is presented.</summary>
        [DebuggerBrowsable(Never)] const string CrewAndCastPage = "fullcredits";
        /// <summary><see cref="Uri" /> format for IMDB movie main information page.</summary>
        /// <remarks>Example format "http://www.imdb.com/title/tt{ID}/?{Query}". Where {ID} is number and {Query} is <see cref="Uri.Query" />.</remarks>
        [DebuggerBrowsable(Never)] const string ImdbUrlFormat = @"^(?i)https?://www.imdb.com/title/tt\d+/?(\??|\?.*)$";

        /// <summary>Initializes new <see cref="ImdbFilmUrl" /> instance.</summary>
        /// <param name="uriString">A URI.</param>
        ImdbFilmUrl(string uriString) : base(uriString) {}

        /// <summary><see cref="Uri" /> from which cast and crew information can be obtained.</summary>
        /// <value>Gets <see cref="Uri" /> from which cast and crew information can be obtained.</value>
        internal Uri CrewAndCastUrl { [DebuggerStepThrough] get => new Uri(new Uri(Invariant($"{ToString().TrimEnd('/')}/")), CrewAndCastPage); }

        /// <summary>Value used for <see cref="DebuggerDisplayAttribute" />.</summary>
        /// <value>Gets value used for <see cref="DebuggerDisplayAttribute" />.</value>
        [DebuggerBrowsable(Never)]
        // ReSharper disable once UnusedMember.Local
        string DebuggerDisplay { [ExcludeFromCodeCoverage] get => Invariant($"{ToString()}; {nameof(CrewAndCastUrl)}: {CrewAndCastUrl}"); }

        /// <summary>Create IMDB movie URL.</summary>
        /// <param name="uriString">An URL to IMDB movie.</param>
        /// <returns>IMDB movie URL.</returns>
        internal static Result<ImdbFilmUrl> CreateUrl(string uriString) => TryCreate(uriString, Absolute, out var _).Negation() ? Fail<ImdbFilmUrl>(String_not_URL) :
            Match(uriString, ImdbUrlFormat).Success ? Ok(new ImdbFilmUrl(uriString)) : Fail<ImdbFilmUrl>(URL_is_not_IMDB_movie);
    }
}