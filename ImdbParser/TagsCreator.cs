using ImdbParser.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;
using static System.Diagnostics.DebuggerBrowsableState;

namespace ImdbParser {
    /// <summary>Creates <see cref="Movie" /> tags.</summary>
    interface ITagsCreator {
        /// <summary>Creates tags file.</summary>
        /// <param name="movie"><see cref="Movie" /> information.</param>
        /// <param name="languages">Languages of <paramref name="movie" />'s soundtrack.</param>
        /// <param name="subtitles">Languages of <paramref name="movie" />'s subtitles.</param>
        IXDocument CreateTagsFile(Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles);
    }

    /// <summary>Creates <see cref="Movie" /> tags.</summary>
    sealed class TagsCreator : ITagsCreator {
        /// <summary>Main tag used to wrap elements.</summary>
        [DebuggerBrowsable(Never)] const string MainTag = "Simple";
        /// <summary>Separator used to split multiple values.</summary>
        [DebuggerBrowsable(Never)] const string MultipleValuesSeparator = "/";
        /// <summary>Tag used to name attribute.</summary>
        [DebuggerBrowsable(Never)] const string TagName = "Name";
        /// <summary>Tags file header that is inside <see cref="TagsFileHeader" />.</summary>
        [DebuggerBrowsable(Never)] const string TagsFileFirstLevelHeader = "Tag";
        /// <summary>Tags file header tag name.</summary>
        [DebuggerBrowsable(Never)] const string TagsFileHeader = "Tags";
        /// <summary>Tag used to set <see cref="TagName" /> value.</summary>
        [DebuggerBrowsable(Never)] const string TagValue = "String";
        /// <summary>Application settings.</summary>
        readonly ISettings _settings;

        /// <summary>Initialize new <see cref="TagsCreator" /> instance.</summary>
        /// <param name="settings">Application settings.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public TagsCreator(ISettings settings) => _settings = settings;

        /// <summary>Creates tags file.</summary>
        /// <param name="movie"><see cref="Movie" /> information.</param>
        /// <param name="languages">Languages of <paramref name="movie" />'s soundtrack.</param>
        /// <param name="subtitles">Languages of <paramref name="movie" />'s subtitles.</param>
        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "String is saved in lower case in the file.")]
        public IXDocument CreateTagsFile(Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles) => new XDocumentWrapper(new XDocument(new XElement(TagsFileHeader,
            new XElement(TagsFileFirstLevelHeader, new XElement(MainTag, new XElement(TagName, _settings.Tags.Title), new XElement(TagValue, movie.Title)),
                new XElement(MainTag, new XElement(TagName, _settings.Tags.Years), new XElement(TagValue, movie.Years)),
                new XElement(MainTag, new XElement(TagName, _settings.Tags.Genres), new XElement(TagValue, movie.Genres.Join(MultipleValuesSeparator))),
                new XElement(MainTag, new XElement(TagName, _settings.Tags.Directors), new XElement(TagValue, movie.Directors.Join(MultipleValuesSeparator))),
                movie.Writers.Any() ? new XElement(MainTag, new XElement(TagName, _settings.Tags.Writers), new XElement(TagValue, movie.Writers.Join(MultipleValuesSeparator))) : default,
                new XElement(MainTag, new XElement(TagName, _settings.Tags.Cast), new XElement(TagValue, movie.Cast.Join(MultipleValuesSeparator))),
                new XElement(MainTag, new XElement(TagName, _settings.Tags.Languages),
                    new XElement(TagValue, languages.Select(language => language.ToString().ToLowerInvariant()).Join(MultipleValuesSeparator))),
                subtitles.Any() ? new XElement(MainTag, new XElement(TagName, _settings.Tags.Subtitles),
                    new XElement(TagValue, subtitles.Select(language => language.ToString().ToLowerInvariant()).Join(MultipleValuesSeparator))) : default))));
    }
}