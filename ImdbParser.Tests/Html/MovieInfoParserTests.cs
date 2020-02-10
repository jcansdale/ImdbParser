using ImdbParser.Html;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Html {
    /// <summary><see cref="MovieInfoParser" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class MovieInfoParserTests {
#region Constructor tests
        /// <summary><see cref="MovieInfoParser(string)" /> test.</summary>
        /// <param name="html">HTML content to parse.</param>
        [TestCase(""), TestCase(" ")]
        public static void Constructor_HtmlInvalid_ThrowsArgumentException(string html) =>
            That(() => new MovieInfoParser(html), Throws.ArgumentException.With.Matches<ArgumentException>(exception => exception.ParamName == "html"));

        /// <summary><see cref="MovieInfoParser(string)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() => That(new MovieInfoParser("HTML"), Not.Null);
#endregion

#region ParseCoverAsync tests
        /// <summary><see cref="MovieInfoParser.ParseCoverAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ParseCoverAsync_Called_ImageReturned() {
            var getter = new Mock<IHtmlGetter>();
            const string Link = "http://google.com/";
            using var bitmap = new Bitmap(1, 1);
            getter.Setup(self => self.GetPictureAsync(new Uri(Link))).ReturnsAsync(bitmap);

            That(await new MovieInfoParser(Invariant($"HTML <span Inner content class=\"poster\" Inner content src=\"{Link}\" Content"), getter.Object).ParseCoverAsync().ConfigureAwait(default),
                SameAs(bitmap));
        }
#endregion

#region ParseGenres tests
        /// <summary><see cref="MovieInfoParser.ParseGenres" /> test.</summary>
        [Test]
        public static void ParseGenres_GenreNotFound_EmptyCollectionReturned() => That(new MovieInfoParser("HTML").ParseGenres, Empty);

        /// <summary><see cref="MovieInfoParser.ParseGenres" /> test.</summary>
        [Test]
        public static void ParseGenres_SingleGenreFound_SingleGenreReturned() {
            const string Genre = "Genre";

            That(new MovieInfoParser(Invariant($"HTML <div Content class=\"subtext\" Content > Content <a href=\" Content > {Genre} < Content <span Content class=\"ghost\" Content")).ParseGenres,
                EqualTo(new[] { Genre }));
        }

        /// <summary><see cref="MovieInfoParser.ParseGenres" /> test.</summary>
        [Test]
        public static void ParseGenres_MultipleGenresFound_MultipleGenresReturned() {
            const string Genre1 = "Genre1";
            const string Genre2 = "Genre2";

            That(new MovieInfoParser(Invariant($"HTML <div Content class=\"subtext\" Content > Content <a href=\" Content > {Genre1} < Content <<a href=\" Content > {Genre2} < Content <span ") +
                "Content class=\"ghost\" Content").ParseGenres, EqualTo(new[] { Genre1, Genre2 }));
        }
#endregion

#region ParseTitle tests
        /// <summary><see cref="MovieInfoParser.ParseTitle" /> test.</summary>
        [Test]
        public static void ParseTitle_TitleNotFound_EntryStringReturned() => That(new MovieInfoParser("HTML").ParseTitle, EqualTo(string.Empty));

        /// <summary><see cref="MovieInfoParser.ParseTitle" /> test.</summary>
        [Test]
        public static void GetTitle_TitleFound_TitleReturned() {
            const string Title = "Title";

            That(new MovieInfoParser(Invariant($"HTML <h1 Inner content> {Title} <Other Tag")).ParseTitle, EqualTo(Title));
        }

        /// <summary><see cref="MovieInfoParser.ParseTitle" /> test.</summary>
        [Test]
        public static void ParseTitle_HasOriginalTitle_OriginalTitleReturned() {
            const string Title = "Original title";

            That(new MovieInfoParser(Invariant($"HTML <h1 Inner content> Title <Other Tag<div Inner content class=\"originalTitle\" Inner content> {Title} <Other Tag")).ParseTitle, EqualTo(Title));
        }

        /// <summary><see cref="MovieInfoParser.ParseTitle" /> test.</summary>
        [Test]
        public static void ParseTitle_EncodedHtmlPassed_HtmlDecoded() => That(new MovieInfoParser("HTML <h1 Inner content>&gt;&nbsp;<Other Tag").ParseTitle, EqualTo(">"));
#endregion

#region ParseYears tests
        /// <summary><see cref="MovieInfoParser.ParseYears" /> test.</summary>
        [Test]
        public static void ParseYears_YearsFound_YearsReturned() {
            const int Years = 2000;

            That(new MovieInfoParser(Invariant($"HTML <span Inner content id=\"titleYear\" Inner content> ( {Years} ) </Other Tag")).ParseYears, EqualTo(Years));
        }

        /// <summary><see cref="MovieInfoParser.ParseYears" /> test.</summary>
        [Test]
        public static void ParseYears_YearsWithLink_YearsReturned() {
            const int Years = 2000;

            That(new MovieInfoParser(Invariant($"HTML <span Inner content id=\"titleYear\" Inner content> ( <a Inner content> {Years} </a> ) <Other Tag")).ParseYears, EqualTo(Years));
        }
#endregion
    }
}