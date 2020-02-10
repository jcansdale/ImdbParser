using ImdbParser.Commands;
using ImdbParser.Options;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static ImdbParser.Commands.GatherCommand;
using static ImdbParser.ImdbFilmUrl;
using static ImdbParser.Language;
using static ImdbParser.Options.LanguageOption;
using static System.Array;
using static System.Reflection.Assembly;

namespace ImdbParser.Tests {
    /// <summary>Holds information used by all tests.</summary>
    [ExcludeFromCodeCoverage]
    static class TestsBase {
        /// <summary><see cref="Uri" /> that is valid IMDB URL.</summary>
        internal const string ImdbTestUrl = "http://www.imdb.com/title/tt1";
        /// <summary>Path to use for link.</summary>
        internal static readonly string Path = GetExecutingAssembly().Location;

        /// <summary>Creates valid <see cref="AddCommand" />.</summary>
        /// <returns>Valid <see cref="AddCommand" />.</returns>
        internal static AddCommand CreateAdd() => AddCommand.Create(new[] { Path }).Value;

        /// <summary>Creates valid <see cref="CreateCommand" />.</summary>
        /// <returns>Valid <see cref="CreateCommand" />.</returns>
        internal static CreateCommand CreateCreate() => CreateCommand.Create(new[] { ImdbTestUrl }, new IOption[] { CreateLanguage(), CreateLink() }).Value;

        /// <summary>Creates valid <see cref="ExtractCommand" />.</summary>
        /// <returns>Valid <see cref="ExtractCommand" />.</returns>
        internal static ExtractCommand CreateExtract() => ExtractCommand.Create(new[] { Path }).Value;

        /// <summary>Creates valid <see cref="GatherCommand" />.</summary>
        /// <returns>Valid <see cref="GatherCommand" />.</returns>
        internal static GatherCommand CreateGather() => Create(new[] { ImdbTestUrl }, new[] { CreateLanguage() }).Value;

        /// <summary>Creates valid <see cref="ImdbFilmUrl" />.</summary>
        /// <returns>Valid <see cref="ImdbFilmUrl" />.</returns>
        internal static ImdbFilmUrl CreateImdbFilmUrl() => CreateUrl(ImdbTestUrl).Value;

        /// <summary>Creates valid <see cref="LanguageOption" />.</summary>
        /// <returns>Valid <see cref="LanguageOption" />.</returns>
        internal static LanguageOption CreateLanguage() => CreateOption(new[] { Lt.ToString() }).Value;

        /// <summary>Creates valid <see cref="LinkOption" />.</summary>
        /// <returns>Valid <see cref="LinkOption" />.</returns>
        internal static LinkOption CreateLink() => LinkOption.CreateOption(new[] { Path }).Value;

        /// <summary>Creates valid <see cref="Movie" />.</summary>
        /// <returns>Valid <see cref="Movie" />.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller should dispose the object.")]
        internal static Movie CreateMovie() => Movie.CreateMovie("Title", 1, new[] { "Genre" }, new Mock<IImage>().Object, new[] { "Director" }, Empty<string>(), new[] { "Actor" }).Value;

        /// <summary>Creates valid <see cref="RemoveCommand" />.</summary>
        /// <returns>Valid <see cref="RemoveCommand" />.</returns>
        internal static RemoveCommand CreateRemove() => RemoveCommand.Create(new[] { Path }).Value;

        /// <summary>Creates valid <see cref="RenameCommand" />.</summary>
        /// <returns>Valid <see cref="RenameCommand" />.</returns>
        internal static RenameCommand CreateRename() => RenameCommand.Create(new[] { Path, Path }).Value;

        /// <summary>Creates valid <see cref="SubtitlesOption" />.</summary>
        /// <returns>Valid <see cref="SubtitlesOption" />.</returns>
        internal static SubtitlesOption CreateSubtitles() => SubtitlesOption.CreateOption(new[] { Lt.ToString() }).Value;
    }
}