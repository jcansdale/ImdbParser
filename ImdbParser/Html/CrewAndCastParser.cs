using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Net.WebUtility;
using static System.StringComparison;

namespace ImdbParser.Html {
    /// <summary>Parses IMDB cast and crew page HTML content.</summary>
    interface ICrewAndCastParser {
        /// <summary>Parses cast from HTML passed.</summary>
        /// <returns>Cast from HTML passed.</returns>
        IEnumerable<string> ParseCast();

        /// <summary>Parses directors from HTML passed.</summary>
        /// <returns>Directors from HTML passed.</returns>
        IEnumerable<string> ParseDirectors();

        /// <summary>Parses writers from HTML passed.</summary>
        /// <returns>Writers from HTML passed.</returns>
        IEnumerable<string> ParseWriters();
    }

    /// <summary>Parses IMDB cast and crew page HTML content.</summary>
    sealed class CrewAndCastParser : HtmlParser, ICrewAndCastParser {
        /// <summary>Text on which multiple actors can be separated.</summary>
        [DebuggerBrowsable(Never)] const string CastElementsSeparator = "<td>";
        /// <summary>Text after which start searching for cast.</summary>
        [DebuggerBrowsable(Never)] const string CastSearchStartText = "class=\"cast_list\"";
        /// <summary>Text after which start searching for directors.</summary>
        [DebuggerBrowsable(Never)] const string DirectorsSearchStartText = "Directed by";
        /// <summary>Text that identifies link start.</summary>
        [DebuggerBrowsable(Never)] const string LinkStartText = "href=";
        /// <summary>Text that identifies class named 'name'.</summary>
        [DebuggerBrowsable(Never)] const string NameClass = "class=\"name\"";
        /// <summary>Tag that specifies table column end.</summary>
        [DebuggerBrowsable(Never)] const string TableColumnEndTag = "</td>";
        /// <summary>Tag that specifies table end.</summary>
        [DebuggerBrowsable(Never)] const string TableEndTag = "</table>";
        /// <summary>Text after which start searching for writers.</summary>
        [DebuggerBrowsable(Never)] const string WritersSearchStartText = "Writing Credits";
        /// <summary>HTML content to parse.</summary>
        readonly string _html;

        /// <summary>Initialize new <see cref="CrewAndCastParser" /> instance.</summary>
        /// <param name="html">HTML content to parse.</param>
        /// <exception cref="ArgumentException"><paramref name="html" /> is empty <see cref="string" /> or contains only white spaces.</exception>
        internal CrewAndCastParser(string html) => _html = HtmlDecode(html.ThrowIfNullOrWhiteSpace(nameof(html)));

        /// <summary>Parses cast from HTML passed.</summary>
        /// <returns>Cast from HTML passed.</returns>
        public IEnumerable<string> ParseCast() => ParseMultipleElementsForCast(CastSearchStartText, CastElementsSeparator);

        /// <summary>Parses directors from HTML passed.</summary>
        /// <returns>Directors from HTML passed.</returns>
        public IEnumerable<string> ParseDirectors() => ParseMultipleElements(DirectorsSearchStartText, NameClass);

        /// <summary>Parses writers from HTML passed.</summary>
        /// <returns>Writers from HTML passed.</returns>
        public IEnumerable<string> ParseWriters() => ParseMultipleElements(WritersSearchStartText, NameClass);

        /// <summary>Gets start for parsing.</summary>
        /// <param name="start">Search start text.</param>
        /// <param name="separator">Text used to split multiple values.</param>
        /// <returns>Text in which elements should be looked for.</returns>
        IEnumerable<string> GetStart(string start, string separator) => _html.IndexOf(start, OrdinalIgnoreCase).MapIf(index => index == -1, _ => Enumerable.Empty<string>(),
            index => _html.Skip(index).ToImmutableList().Map(html => html.Join().IndexOf(TableEndTag, OrdinalIgnoreCase).Map(html.Take)).Join().Split(separator).Skip(1));

        /// <summary>Parses multiple elements starting to look from <paramref name="start" />.</summary>
        /// <param name="start">Search start text.</param>
        /// <param name="separator">Text used to split multiple values.</param>
        /// <returns>Names in given content block.</returns>
        IEnumerable<string> ParseMultipleElements(string start, string separator) => GetStart(start, separator).Select(part =>
            part.Split(TableColumnEndTag)[0].IndexOf(LinkStartText, OrdinalIgnoreCase).Map(index => SkipFirstCharacterTakeWhileAndJoin(index == -1 ? part : part.Skip(index))));

        /// <summary>Parses multiple elements for cast starting to look from <paramref name="start" />.</summary>
        /// <param name="start">Search start text.</param>
        /// <param name="separator">Text used to split multiple values.</param>
        /// <returns>Names in given content block.</returns>
        IEnumerable<string> ParseMultipleElementsForCast(string start, string separator) => GetStart(start, separator).Select(part =>
            part.Split(TableColumnEndTag)[0].IndexOf(LinkStartText, OrdinalIgnoreCase).
                MapIf(index => index == -1, _ => TakeWhileAndJoin(part).Trim(), index => SkipFirstCharacterTakeWhileAndJoin(part.Skip(index))));
    }
}