using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Net.WebUtility;
using static System.String;
using static System.StringComparison;

namespace ImdbParser.Html {
    /// <summary>Parses IMDB movie page HTML content.</summary>
    interface IMovieInfoParser {
        /// <summary>Parses cover from HTML passed.</summary>
        /// <returns>Cover from HTML passed.</returns>
        Task<Image> ParseCoverAsync();

        /// <summary>Parses genres from HTML passed.</summary>
        /// <returns>Genres from HTML passed.</returns>
        IEnumerable<string> ParseGenres();

        /// <summary>Parses years from HTML passed.</summary>
        /// <returns>Years from HTML passed.</returns>
        int ParseYears();

        /// <summary>Parses title from HTML passed.</summary>
        /// <returns>Titles from HTML passed.</returns>
        string ParseTitle();
    }

    /// <summary>Parses IMDB movie page HTML content.</summary>
    sealed class MovieInfoParser : HtmlParser, IMovieInfoParser {
        /// <summary>Text until which to search for genres.</summary>
        [DebuggerBrowsable(Never)] const string GenresEnd = "class=\"ghost\"";
        /// <summary>Genres start separator.</summary>
        [DebuggerBrowsable(Never)] const string GenresStart = "<a href=";
        /// <summary>Header one start text.</summary>
        [DebuggerBrowsable(Never)] const string Header1TagStart = "<h1";
        /// <summary>Years start separator.</summary>
        [DebuggerBrowsable(Never)] const string YearsStart = "id=\"titleYear\"";
        /// <summary>Image start separator.</summary>
        [DebuggerBrowsable(Never)] const string ImageStart = "class=\"poster\"";
        /// <summary>Source tag.</summary>
        [DebuggerBrowsable(Never)] const string SourceAttribute = "src";
        /// <summary>Separator used to separate <see cref="string" />.</summary>
        [DebuggerBrowsable(Never)] const char StringSeparator = '"';
        /// <summary>Location where additional information, such as date and genres starts.</summary>
        [DebuggerBrowsable(Never)] const string SubTextStart = "class=\"subtext\"";
        /// <summary>Symbol used to mark tag opening.</summary>
        [DebuggerBrowsable(Never)] const char TagOpenSymbol = '<';
        /// <summary>Symbol used before tag close symbol.</summary>
        [DebuggerBrowsable(Never)] const char TagPreCloseSymbol = '/';
        /// <summary>Title start separator.</summary>
        [DebuggerBrowsable(Never)] const string TitlesStart = "class=\"originalTitle\"";
        /// <summary>Symbols on which trimming should be performed.</summary>
        [DebuggerBrowsable(Never)] static readonly char[] SymbolsToTrim = { ' ', '(', ')' };
        /// <summary>HTML content getter.</summary>
        readonly IHtmlGetter _getter;
        /// <summary>HTML content to parse.</summary>
        readonly string _html;

        /// <summary>Initialize new <see cref="MovieInfoParser" /> instance.</summary>
        /// <param name="html">HTML content to parse.</param>
        /// <exception cref="ArgumentException"><paramref name="html" /> is empty or white spaced.</exception>
        internal MovieInfoParser(string html) : this(html, new HtmlGetter()) {}

        /// <summary>Initialize new <see cref="MovieInfoParser" /> instance.</summary>
        /// <param name="html">HTML content to parse.</param>
        /// <param name="getter">HTML content getter.</param>
        /// <exception cref="ArgumentException"><paramref name="html" /> is empty <see cref="string" /> or contains only white spaces.</exception>
        internal MovieInfoParser(string html, IHtmlGetter getter) {
            // ReSharper disable once PossibleNullReferenceException
            _html = HtmlDecode(html.ThrowIfNullOrWhiteSpace(nameof(html))).Replace("&nbsp;", Empty, InvariantCulture);
            _getter = getter;
        }

        /// <summary>Parses multiple values from <paramref name="enumerable" /> based on <paramref name="separator" />.</summary>
        /// <param name="enumerable">Collection to get multiple values from.</param>
        /// <param name="separator">Separator used.</param>
        /// <returns>Values found in <paramref name="enumerable" />.</returns>
        static IEnumerable<string> GetMultipleValues(IEnumerable<char> enumerable, string separator) =>
            Concat(enumerable).Split(separator).Skip(1).Select(value => SkipFirstCharacterTakeWhileAndJoin(value));

        /// <summary>Parses cover from HTML passed.</summary>
        /// <returns>Cover from HTML passed.</returns>
        public Task<Image> ParseCoverAsync() => _html.IndexOf(ImageStart, OrdinalIgnoreCase).Map(index => _html.IndexOf(SourceAttribute, index, OrdinalIgnoreCase)).Map(_html.Skip).
            Map(coverHtml => SkipFirstCharacterTakeWhileAndJoin(coverHtml, StringSeparator, StringSeparator)).Map(uri => _getter.GetPictureAsync(new Uri(uri)));

        /// <summary>Parses genres from HTML passed.</summary>
        /// <returns>Genres from HTML passed.</returns>
        public IEnumerable<string> ParseGenres() => _html.IndexOf(SubTextStart, OrdinalIgnoreCase).Map(_html.Skip).Join().
            Map(subTextHtml => subTextHtml.IndexOf(GenresStart, OrdinalIgnoreCase).Map(subTextHtml.Skip)).ToImmutableList().
            Map(genresHtml => Concat(genresHtml).IndexOf(GenresEnd, OrdinalIgnoreCase).Map(genresHtml.Take)).Map(html => GetMultipleValues(html, GenresStart));

        /// <summary>Parses years from HTML passed.</summary>
        /// <returns>Years from HTML passed.</returns>
        public int ParseYears() => _html.IndexOf(YearsStart, OrdinalIgnoreCase).Map(_html.Skip).Map(yearsHtml => SkipFirstCharacterFound(yearsHtml)).ToImmutableList().
            Map(yearsHtml => SkipFirstCharacterFound(yearsHtml, TagOpenSymbol).ToImmutableList().
                Map(link => link.FirstOrDefault() == TagPreCloseSymbol ? yearsHtml : link.Map(linkHtml => SkipFirstCharacterFound(linkHtml)))).Map(yearsHtml => TakeWhileAndJoin(yearsHtml)).
            Trim(SymbolsToTrim).Map(years => Convert.ToInt32(years, CultureInfo.InvariantCulture));

        /// <summary>Parses title from HTML passed.</summary>
        /// <returns>Titles from HTML passed.</returns>
        public string ParseTitle() => _html.IndexOf(TitlesStart, OrdinalIgnoreCase).MapIf(index => index == -1, () => _html.IndexOf(Header1TagStart, OrdinalIgnoreCase)).Map(_html.Skip).
            Map(titleHtml => SkipFirstCharacterTakeWhileAndJoin(titleHtml));
    }
}