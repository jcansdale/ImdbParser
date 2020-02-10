using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static ImdbParser.Movie;

namespace ImdbParser.Html {
    /// <summary>Gets and parses IMDB pages HTML.</summary>
    interface IHtmlManager {
        /// <summary>Parses movie information from <paramref name="url" />.</summary>
        /// <param name="url">URL from which information should be downloaded.</param>
        /// <returns>Movie information <paramref name="url" />.</returns>
        Task<DisposableResult<Movie>> ParseMovieAsync(ImdbFilmUrl url);
    }

    /// <summary>Gets and parses IMDB pages HTML.</summary>
    sealed class HtmlManager : IHtmlManager {
        /// <summary>IMDB pages parsers creator.</summary>
        readonly IInfoParserFactory _factory;
        /// <summary>HTML content getter.</summary>
        readonly IHtmlGetter _getter;

        /// <summary>Initialize new <see cref="HtmlManager" /> instance.</summary>
        /// <param name="getter">HTML content getter.</param>
        /// <param name="factory">IMDB pages parsers creator.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public HtmlManager(IHtmlGetter getter, IInfoParserFactory factory) {
            _getter = getter;
            _factory = factory;
        }

        /// <summary>Parses movie information from <paramref name="url" />.</summary>
        /// <param name="url">URL from which information should be downloaded.</param>
        /// <returns>Movie information <paramref name="url" />.</returns>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller should dispose the object.")]
        public async Task<DisposableResult<Movie>> ParseMovieAsync(ImdbFilmUrl url) {
            var movieHtml = _getter.GetHtmlAsync(url);
            var crewAndCastHtml = _getter.GetHtmlAsync(url.CrewAndCastUrl);
            var movieInfoParser = _factory.CreateMovieInfoParser(await movieHtml.ConfigureAwait(default));
            var cover = movieInfoParser.ParseCoverAsync();
            var crewAndCastInfoParser = _factory.CreateCrewAndCastParser(await crewAndCastHtml.ConfigureAwait(default));
            return CreateMovie(movieInfoParser.ParseTitle(), movieInfoParser.ParseYears(), movieInfoParser.ParseGenres().ToList(), new ImageWrapper(await cover.ConfigureAwait(default)),
                crewAndCastInfoParser.ParseDirectors().ToList(), crewAndCastInfoParser.ParseWriters(), crewAndCastInfoParser.ParseCast().ToList());
        }
    }
}