using ImdbParser.Settings;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static ImdbParser.Language;
using static ImdbParser.Movie;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.FormattableString;

namespace ImdbParser.Tests {
    /// <summary><see cref="TagsCreator" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class TagsCreatorTests {
#region Constructor tests
        /// <summary><see cref="TagsCreator(ISettings)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() => That(new TagsCreator(Setup().settings.Object), Not.Null);
#endregion

#region CreateTagsFile tests
        /// <summary><see cref="TagsCreator.CreateTagsFile" /> test.</summary>
        [Test]
        public static void CreateTagsFile_MinimalMovie_FileCreated() {
            var setup = Setup();
            setup.settings.SetupSettings();
            using var movie = CreateMovie();

            That(setup.creator.CreateTagsFile(movie, Empty<Language>(), Empty<Language>()).ToString, EqualTo(@"<Tags>
  <Tag>
    <Simple>
      <Name>Title tag</Name>
      <String>Title</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Genre</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Director</String>
    </Simple>
    <Simple>
      <Name>Cast</Name>
      <String>Actor</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String></String>
    </Simple>
  </Tag>
</Tags>"));
        }

        /// <summary><see cref="TagsCreator.CreateTagsFile" /> test.</summary>
        [Test]
        public static void CreateTagsFile_MovieWithMultipleValuesPassed_FileCreated() {
            var setup = Setup();
            setup.settings.SetupSettings();
            const string Title = "Title";
            const int Years = 1;
            const string Genre1 = "Genre1";
            const string Genre2 = "Genre2";
            const string Director1 = "Director1";
            const string Director2 = "Director2";
            const string Writer1 = "Writer1";
            const string Writer2 = "Writer2";
            const string Actor1 = "Actor1";
            const string Actor2 = "Actor2";
            using var movie = CreateMovie(Title, Years, new[] { Genre1, Genre2 }, new Mock<IImage>().Object, new[] { Director1, Director2 }, new[] { Writer1, Writer2 }, new[] { Actor1, Actor2 });

            That(setup.creator.CreateTagsFile(movie.Value, Empty<Language>(), Empty<Language>()).ToString, EqualTo(Invariant($@"<Tags>
  <Tag>
    <Simple>
      <Name>Title tag</Name>
      <String>{Title}</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>{Years}</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>{Genre1}/{Genre2}</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>{Director1}/{Director2}</String>
    </Simple>
    <Simple>
      <Name>Writers</Name>
      <String>{Writer1}/{Writer2}</String>
    </Simple>
    <Simple>
      <Name>Cast</Name>
      <String>{Actor1}/{Actor2}</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String></String>
    </Simple>
  </Tag>
</Tags>")));
        }

        /// <summary><see cref="TagsCreator.CreateTagsFile" /> test.</summary>
        [Test]
        public static void CreateTagsFile_LanguagesAndSubtitlesPassed_LanguagesAndSubtitlesAdded() {
            var setup = Setup();
            setup.settings.SetupSettings();
            var languages = new[] { Lt, En };
            using var movie = CreateMovie();

            That(setup.creator.CreateTagsFile(movie, languages, languages).ToString, EqualTo(@"<Tags>
  <Tag>
    <Simple>
      <Name>Title tag</Name>
      <String>Title</String>
    </Simple>
    <Simple>
      <Name>Years</Name>
      <String>1</String>
    </Simple>
    <Simple>
      <Name>Genres</Name>
      <String>Genre</String>
    </Simple>
    <Simple>
      <Name>Directors</Name>
      <String>Director</String>
    </Simple>
    <Simple>
      <Name>Cast</Name>
      <String>Actor</String>
    </Simple>
    <Simple>
      <Name>Languages</Name>
      <String>lt/en</String>
    </Simple>
    <Simple>
      <Name>Subtitles</Name>
      <String>lt/en</String>
    </Simple>
  </Tag>
</Tags>"));
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />.</returns>
        static (TagsCreator creator, Mock<ISettings> settings) Setup() {
            var settings = new Mock<ISettings>();
            return (new TagsCreator(settings.Object), settings);
        }

        /// <summary>Sets up <paramref name="settings" /> to return valid values.</summary>
        /// <param name="settings"><see cref="ISettings" /> <see cref="Mock" /> to set up.</param>
        static void SetupSettings(this Mock<ISettings> settings) =>
            settings.SetupGet(self => self.Tags).Returns(new Tags("Cast", "Directors", "Genres", "Years", "Languages", "Subtitles", "Title tag", "Writers"));
    }
}