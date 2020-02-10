using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static ImdbParser.Messages;
using static ImdbParser.Movie;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;

namespace ImdbParser.Tests {
    /// <summary><see cref="Movie" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class MovieTests {
#region CreateMovie tests
        /// <summary><see cref="CreateMovie" /> test.</summary>
        /// <param name="title">Title of the movie.</param>
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), TestCase(null), TestCase(""), TestCase(" ")]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        public static void CreateMovie_NoTitle_FailureReturned(string title) => CreateMovie(title, 0, Empty<string>(), new Mock<IImage>().Object, Empty<string>(), Empty<string>(), Empty<string>()).
            ExpectFailAndDispose(() => Failed_to_get_title);

        /// <summary><see cref="CreateMovie" /> test.</summary>
        /// <param name="years">Years when the movie was released.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), TestCase(-1), TestCase(0)]
        public static void CreateMovie_NoYears_FailureReturned(int years) => CreateMovie("Title", years, Empty<string>(), new Mock<IImage>().Object, Empty<string>(), Empty<string>(), Empty<string>()).
            ExpectFailAndDispose(() => Failed_to_get_years);

        /// <summary><see cref="CreateMovie" /> test.</summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), Test]
        public static void CreateMovie_NoGenres_FailureReturned() => CreateMovie("Title", 1, Empty<string>(), new Mock<IImage>().Object, Empty<string>(), Empty<string>(), Empty<string>()).
            ExpectFailAndDispose(() => Failed_to_get_genres);

        /// <summary><see cref="CreateMovie" /> test.</summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), Test]
        public static void CreateMovie_NoDirectors_FailureReturned() => CreateMovie("Title", 1, new[] { "Genre" }, new Mock<IImage>().Object, Empty<string>(), Empty<string>(), Empty<string>()).
            ExpectFailAndDispose(() => Failed_to_get_directors);

        /// <summary><see cref="CreateMovie" /> test.</summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), Test]
        public static void CreateMovie_NoCast_FailureReturned() => CreateMovie("Title", 1, new[] { "Genre" }, new Mock<IImage>().Object, new[] { "Director" }, Empty<string>(), Empty<string>()).
            ExpectFailAndDispose(() => Failed_to_get_cast);

        /// <summary><see cref="CreateMovie" /> test.</summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), Test]
        public static void CreateMovie_ValidArgumentsPassed_MovieCreated() {
            const string Title = "Title";
            const int Years = 1;
            var genres = new[] { "Genre" };
            var directors = new[] { "Director" };
            var writers = new[] { "Writer" };
            var cast = new[] { "Actor" };

            CreateMovie(Title, Years, genres, new Mock<IImage>().Object, directors, writers, cast).ExpectSuccessAndDispose(result => {
                using var movie = result;
                Multiple(() => {
                    // ReSharper disable AccessToDisposedClosure
                    That(movie.Title, SameAs(Title));
                    That(movie.Years, EqualTo(Years));
                    That(movie.Genres, SameAs(genres));
                    That(movie.Directors, SameAs(directors));
                    That(movie.Writers, SameAs(writers));
                    That(movie.Cast, SameAs(cast));
                    // ReSharper restore AccessToDisposedClosure
                });
            });
        }

        /// <summary><see cref="CreateMovie" /> test.</summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), Test]
        public static void CreateMovie_WritersNotPassed_WritersNotSet() =>
            CreateMovie("Title", 1, new[] { "Genre" }, new Mock<IImage>().Object, new[] { "Director" }, Empty<string>(), new[] { "Actor" }).ExpectSuccessAndDispose(result => {
                using var movie = result;
                That(movie.Writers, Is.Empty);
            });
#endregion

#region Cover tests
        /// <summary><see cref="Movie.Cover" /> test.</summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive."), Test]
        public static void GetCover_Called_CoverReturned() {
            var cover = new Mock<IImage>().Object;

            CreateMovie("Title", 1, new[] { "Genre" }, cover, new[] { "Director" }, Empty<string>(), new[] { "Actor" }).ExpectSuccessAndDispose(result => {
                using var movie = result;
                That(movie.Cover, SameAs(cover));
            });
        }

        /// <summary><see cref="Movie.Cover" /> test.</summary>
        [Test]
        public static void Cover_CalledOnDisposedObject_Throws() {
            var movie = TestsBase.CreateMovie();
            movie.Dispose();

            Throws<ObjectDisposedException>(() => movie.Cover());
        }
#endregion

#region Dispose tests
        /// <summary><see cref="Movie.Dispose" /> tests.</summary>
        [Test]
        public static void Dispose_CalledMultipleTimes_DoesNotThrow() {
            using var movie = TestsBase.CreateMovie();

            movie.Dispose();

            DoesNotThrow(movie.Dispose);
        }
#endregion
    }
}