using ImdbParser.Settings;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.IO.Path;

namespace ImdbParser.Tests.Settings {
    /// <summary><see cref="ApplicationSettings" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ApplicationSettingsTests {
#region Constructor tests
        /// <summary><see cref="ApplicationSettings()" /> test.</summary>
        [Test]
        public static void Constructor_Called_NoExceptionThrown() => That(new ApplicationSettings(), Not.Null);
#endregion

#region CoverName tests
        /// <summary><see cref="ApplicationSettings.CoverName" /> test.</summary>
        [Test]
        public static void CoverName_Called_ValidValueReturned() {
            var setup = Setup();
            const string MoviesDirectory = @"C:\Value";
            setup.root.SetupRootIndexer("MoviesDirectory", MoviesDirectory);
            const string LibraryDirectory = @"Library\Directory";
            setup.root.SetupRootIndexer("LibraryDirectory", LibraryDirectory);
            const string CoverName = "Cover.extension";
            setup.root.SetupRootIndexer("CoverName", CoverName);

            That(setup.settings.CoverName.FullName, EqualTo(Combine(MoviesDirectory, LibraryDirectory, CoverName)));
        }
#endregion

#region Folders tests
        /// <summary><see cref="ApplicationSettings.Folders" /> test.</summary>
        [Test]
        public static void TagsFolders_Called_SettingsReturned() {
            var setup = Setup();
            var section = new Mock<IConfigurationSection>();
            const string Cast = "Cast";
            const string CastValue = "CastValue";
            var cast = CreateSection(Cast, CastValue).Object;
            const string Directors = "Directors";
            const string DirectorsValue = "DirectorsValue";
            var directors = CreateSection(Directors, DirectorsValue).Object;
            const string Genres = "Genres";
            const string GenresValue = "GenresValue";
            var genres = CreateSection(Genres, GenresValue).Object;
            const string Years = "Years";
            const string YearsValue = "YearsValue";
            var years = CreateSection(Years, YearsValue).Object;
            section.Setup(self => self.GetChildren()).Returns(new[] { cast, directors, genres, years });
            section.SetupSectionGetSection(Cast, cast);
            section.SetupSectionGetSection(Directors, directors);
            section.SetupSectionGetSection(Genres, genres);
            section.SetupSectionGetSection(Years, years);
            setup.root.Setup(self => self.GetSection("Folders")).Returns(section.Object);

            Multiple(() => {
                That(setup.settings.Folders.Cast, SameAs(CastValue));
                That(setup.settings.Folders.Directors, SameAs(DirectorsValue));
                That(setup.settings.Folders.Genres, SameAs(GenresValue));
                That(setup.settings.Folders.Years, SameAs(YearsValue));
            });
        }
#endregion

#region LibraryDirectory tests
        /// <summary><see cref="ApplicationSettings.LibraryDirectory" /> test.</summary>
        [Test]
        public static void LibraryDirectory_Called_ValidValueReturned() {
            var setup = Setup();
            const string MoviesDirectory = @"C:\Value";
            setup.root.SetupRootIndexer("MoviesDirectory", MoviesDirectory);
            const string LibraryDirectory = @"Library\Directory";
            setup.root.SetupRootIndexer("LibraryDirectory", LibraryDirectory);

            That(setup.settings.LibraryDirectory.FullName, EqualTo(Combine(MoviesDirectory, LibraryDirectory)));
        }
#endregion

#region MoviesDirectory tests
        /// <summary><see cref="ApplicationSettings.MoviesDirectory" /> test.</summary>
        [Test]
        public static void MoviesDirectory_Called_SettingReturned() {
            var setup = Setup();
            const string Directory = @"C:\Value";
            setup.root.SetupRootIndexer("MoviesDirectory", Directory);

            That(setup.settings.MoviesDirectory.FullName, SameAs(Directory));
        }
#endregion

#region Tags tests
        /// <summary><see cref="ApplicationSettings.Tags" /> test.</summary>
        [Test]
        public static void Tags_Called_SettingsReturned() {
            var setup = Setup();
            var section = new Mock<IConfigurationSection>();
            const string Cast = "Cast";
            const string CastValue = "CastValue";
            var cast = CreateSection(Cast, CastValue).Object;
            const string Directors = "Directors";
            const string DirectorsValue = "DirectorsValue";
            var directors = CreateSection(Directors, DirectorsValue).Object;
            const string Genres = "Genres";
            const string GenresValue = "GenresValue";
            var genres = CreateSection(Genres, GenresValue).Object;
            const string Languages = "Languages";
            const string LanguagesValue = "LanguagesValue";
            var languages = CreateSection(Languages, LanguagesValue).Object;
            const string Subtitles = "Subtitles";
            const string SubtitlesValue = "SubtitlesValue";
            var subtitles = CreateSection(Subtitles, SubtitlesValue).Object;
            const string Title = "Title";
            const string TitleValue = "TitleValue";
            var title = CreateSection(Title, TitleValue).Object;
            const string Writers = "Writers";
            const string WritersValue = "WritersValue";
            var writers = CreateSection(Writers, WritersValue).Object;
            const string Years = "Years";
            const string YearsValue = "YearsValue";
            var years = CreateSection(Years, YearsValue).Object;
            section.Setup(self => self.GetChildren()).Returns(new[] { cast, directors, genres, languages, subtitles, title, writers, years });
            section.SetupSectionGetSection(Cast, cast);
            section.SetupSectionGetSection(Directors, directors);
            section.SetupSectionGetSection(Genres, genres);
            section.SetupSectionGetSection(Languages, languages);
            section.SetupSectionGetSection(Subtitles, subtitles);
            section.SetupSectionGetSection(Title, title);
            section.SetupSectionGetSection(Writers, writers);
            section.SetupSectionGetSection(Years, years);
            setup.root.Setup(self => self.GetSection("Tags")).Returns(section.Object);

            Multiple(() => {
                That(setup.settings.Tags.Cast, SameAs(CastValue));
                That(setup.settings.Tags.Directors, SameAs(DirectorsValue));
                That(setup.settings.Tags.Genres, SameAs(GenresValue));
                That(setup.settings.Tags.Languages, SameAs(LanguagesValue));
                That(setup.settings.Tags.Subtitles, SameAs(SubtitlesValue));
                That(setup.settings.Tags.Title, SameAs(TitleValue));
                That(setup.settings.Tags.Writers, SameAs(WritersValue));
                That(setup.settings.Tags.Years, SameAs(YearsValue));
            });
        }
#endregion

#region TagsName tests
        /// <summary><see cref="ApplicationSettings.TagsName" /> test.</summary>
        [Test]
        public static void TagsName_Called_ValidValueReturned() {
            var setup = Setup();
            const string MoviesDirectory = @"C:\Value";
            setup.root.SetupRootIndexer("MoviesDirectory", MoviesDirectory);
            const string LibraryDirectory = @"Library\Directory";
            setup.root.SetupRootIndexer("LibraryDirectory", LibraryDirectory);
            const string TagsName = "Tags.extension";
            setup.root.SetupRootIndexer("TagsName", TagsName);

            That(setup.settings.TagsName.FullName, EqualTo(Combine(MoviesDirectory, LibraryDirectory, TagsName)));
        }
#endregion

        /// <summary>Creates <see cref="IConfigurationSection" /> <see cref="Mock" />.</summary>
        /// <param name="key">Key to return from <see cref="IConfigurationSection.Key" />.</param>
        /// <param name="value">Value to return from <see cref="IConfigurationSection.Value" />.</param>
        /// <returns><see cref="IConfigurationSection" /> <see cref="Mock" />.</returns>
        static Mock<IConfigurationSection> CreateSection(string key, string value) {
            var section = new Mock<IConfigurationSection>();
            section.SetupGet(self => self.Key).Returns(key);
            section.SetupGet(self => self.Value).Returns(value);
            return section;
        }

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />.</returns>
        static (ApplicationSettings settings, Mock<IConfigurationRoot> root) Setup() {
            var root = new Mock<IConfigurationRoot>();
            return (new ApplicationSettings(root.Object), root);
        }

        /// <summary>Sets up <paramref name="root" /> to return <paramref name="value" /> from <see cref="IConfiguration.this" /> call with <paramref name="value" />.</summary>
        /// <param name="root"><see cref="IConfigurationRoot" /> <see cref="Mock" /> to set up.</param>
        /// <param name="index">Index to expect.</param>
        /// <param name="value">Value to return.</param>
        static void SetupRootIndexer(this Mock<IConfigurationRoot> root, string index, string value) => root.SetupGet(self => self[index]).Returns(value);

        /// <summary>Sets up <paramref name="section" /> to return <paramref name="innerSection" /> from <see cref="IConfiguration.GetSection" /> call with <paramref name="key" />.</summary>
        /// <param name="section"><see cref="IConfigurationSection" /> <see cref="Mock" /> to set up.</param>
        /// <param name="key">Key to expect.</param>
        /// <param name="innerSection">Value to return.</param>
        static void SetupSectionGetSection(this Mock<IConfigurationSection> section, string key, IConfigurationSection innerSection) =>
            section.Setup(self => self.GetSection(key)).Returns(innerSection);
    }
}