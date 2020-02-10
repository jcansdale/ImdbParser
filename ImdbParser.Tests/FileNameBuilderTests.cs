using ImdbParser.Settings;
using Moq;
using NUnit.Framework;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using static ImdbParser.Language;
using static ImdbParser.Movie;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using static System.IO.Path;
using static System.Linq.Enumerable;
using static System.String;

namespace ImdbParser.Tests {
    /// <summary><see cref="FileNameBuilder" /> test.</summary>
    [ExcludeFromCodeCoverage]
    static class FileNameBuilderTests {
#region Constructor tests
        /// <summary><see cref="FileNameBuilder(ISettings)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new FileNameBuilder(Setup().settings.Object), Not.Null);
#endregion

#region BuildFileName tests
        /// <summary><see cref="FileNameBuilder.BuildFileName" /> test.</summary>
        [Test]
        public static void BuildFileName_NoSubtitles_NameWithoutSubtitlesReturned() {
            var setup = Setup();
            setup.settings.SetupSettings();
            using var movie = CreateMovie();

            That(setup.builder.BuildFileName(movie, new[] { Lt }, Empty<Language>().ToImmutableList()).ToString, EqualTo(@"A:\B\Title (1); Genre; LT.mkv"));
        }

        /// <summary><see cref="FileNameBuilder.BuildFileName" /> test.</summary>
        [Test]
        public static void BuildFileName_MultipleGenres_ValuesSeparated() {
            var setup = Setup();
            setup.settings.SetupSettings();
            const string Title = "Title";
            const int Years = 1;
            const string Genre1 = "Genre1";
            const string Genre2 = "Genre2";
            using var movie = CreateMovie(Title, Years, new[] { Genre1, Genre2 }, new Mock<IImage>().Object, new[] { "Director" }, Empty<string>(), new[] { "Actor" });

            That(setup.builder.BuildFileName(movie.Value, new[] { Lt }, Empty<Language>().ToImmutableList()).ToString, EqualTo(Invariant($@"A:\B\{Title} ({Years}); {Genre1}, {Genre2}; LT.mkv")));
        }

        /// <summary><see cref="FileNameBuilder.BuildFileName" /> test.</summary>
        [Test]
        public static void BuildFileName_MultipleLanguages_ValuesSeparated() {
            var setup = Setup();
            setup.settings.SetupSettings();
            using var movie = CreateMovie();

            That(setup.builder.BuildFileName(movie, new[] { Lt, En }, Empty<Language>().ToImmutableList()).ToString, EqualTo(@"A:\B\Title (1); Genre; LT, EN.mkv"));
        }

        /// <summary><see cref="FileNameBuilder.BuildFileName" /> test.</summary>
        [Test]
        public static void BuildFileName_HasSubtitles_SubtitlesAdded() {
            var setup = Setup();
            setup.settings.SetupSettings();
            using var movie = CreateMovie();

            That(setup.builder.BuildFileName(movie, new[] { Lt, En }, new[] { Lt, En }).ToString, EqualTo(@"A:\B\Title (1); Genre; LT, EN; LT, EN.mkv"));
        }

        /// <summary><see cref="FileNameBuilder.BuildFileName" /> test.</summary>
        [Test]
        public static void BuildFileName_TitleHadInvalidCharacters_InvalidCharactersRemoved() {
            var setup = Setup();
            setup.settings.SetupSettings();
            const string Title = "Title";
            const int Years = 1;
            const string Genre = "Genre";
            using var movie = CreateMovie(Join(string.Empty, GetInvalidFileNameChars().ToArray()) + Title + Join(string.Empty, GetInvalidPathChars().ToArray()), Years, new[] { Genre },
                new Mock<IImage>().Object, new[] { "Director" }, Empty<string>(), new[] { "Actor" });

            That(setup.builder.BuildFileName(movie.Value, new[] { Lt }, Empty<Language>().ToImmutableList()).ToString, EqualTo(Invariant($@"A:\B\{Title} ({Years}); {Genre}; LT.mkv")));
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />.</returns>
        static (FileNameBuilder builder, Mock<ISettings> settings) Setup() {
            var settings = new Mock<ISettings>();
            return (new FileNameBuilder(settings.Object), settings);
        }

        /// <summary>Sets up <paramref name="settings" /> to return valid values.</summary>
        /// <param name="settings"><see cref="ISettings" /> <see cref="Mock" /> to set up.</param>
        static void SetupSettings(this Mock<ISettings> settings) => settings.SetupGet(self => self.MoviesDirectory).Returns(new DirectoryInfo(@"A:\B"));
    }
}