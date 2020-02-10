using ImdbParser.Html;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Html {
    /// <summary><see cref="HtmlManager" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class HtmlManagerTests {
#region Constructor tests
        /// <summary><see cref="HtmlManager(IHtmlGetter, IInfoParserFactory)" /></summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new HtmlManager(setup.getter.Object, setup.factory.Object), Not.Null);
        }
#endregion

#region ParseMovieAsync tests
        /// <summary><see cref="HtmlManager.ParseMovieAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [SuppressMessage("Maintainability", "CA1506:Avoid excessive class coupling", Justification = "Valid for testing."), Test]
        public static async Task ParseMovieAsync_ParseSucceed_MovieReturned() {
            var setup = Setup();
            var url = CreateImdbFilmUrl();
            const string Html1 = "HTML1";
            setup.getter.Setup(self => self.GetHtmlAsync(url)).ReturnsAsync(Html1);
            const string Html2 = "HTML2";
            setup.getter.Setup(self => self.GetHtmlAsync(url.CrewAndCastUrl)).ReturnsAsync(Html2);
            var movieInfoParser = new Mock<IMovieInfoParser>();
            const string Title = "Title";
            movieInfoParser.Setup(self => self.ParseTitle()).Returns(Title);
            const int Years = 1;
            movieInfoParser.Setup(self => self.ParseYears()).Returns(Years);
            var genres = new[] { "Genre" };
            movieInfoParser.Setup(self => self.ParseGenres()).Returns(genres);
            setup.factory.Setup(self => self.CreateMovieInfoParser(Html1)).Returns(movieInfoParser.Object);
            var crewAndCastParser = new Mock<ICrewAndCastParser>();
            var directors = new[] { "Director" };
            crewAndCastParser.Setup(self => self.ParseDirectors()).Returns(directors);
            var writers = new[] { "Writer" };
            crewAndCastParser.Setup(self => self.ParseWriters()).Returns(writers);
            var cast = new[] { "Actor" };
            crewAndCastParser.Setup(self => self.ParseCast()).Returns(cast);
            setup.factory.Setup(self => self.CreateCrewAndCastParser(Html2)).Returns(crewAndCastParser.Object);
            using var cover = new Bitmap(1, 1);
            movieInfoParser.Setup(self => self.ParseCoverAsync()).ReturnsAsync(cover);

            (await setup.manager.ParseMovieAsync(url).ConfigureAwait(default)).ExpectSuccessAndDispose(movie => Multiple(() => {
                That(movie.Cast, EqualTo(cast));
                That(movie.Directors, EqualTo(directors));
                That(movie.Genres, EqualTo(genres));
                That(movie.Title, SameAs(Title));
                That(movie.Writers, SameAs(writers));
                That(movie.Years, EqualTo(Years));
                That(movie.Cover, InstanceOf<IImage>());
            }));
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />s.</returns>
        static (HtmlManager manager, Mock<IHtmlGetter> getter, Mock<IInfoParserFactory> factory) Setup() {
            var getter = new Mock<IHtmlGetter>();
            var factory = new Mock<IInfoParserFactory>();
            return (new HtmlManager(getter.Object, factory.Object), getter, factory);
        }
    }
}