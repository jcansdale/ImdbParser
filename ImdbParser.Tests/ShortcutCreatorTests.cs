using ImdbParser.Settings;
using IWshRuntimeLibrary;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static ImdbParser.Tests.TestsBase;
using static Moq.Times;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.FormattableString;
using static System.IO.Path;

namespace ImdbParser.Tests {
    /// <summary><see cref="ShortcutCreator" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class ShortcutCreatorTests {
#region Constructor tests
        /// <summary><see cref="ShortcutCreator(IFileNameBuilder, IWshShell3, IDirectory, ISettings)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new ShortcutCreator(setup.builder.Object, setup.shell.Object, setup.directory.Object, setup.settings.Object), Not.Null);
        }
#endregion

#region CreateShortcut tests
        /// <summary><see cref="ShortcutCreator.CreateShortcut" /> test.</summary>
        [Test]
        public static void CreateShortcut_Called_ShortcutsCreated() {
            var setup = Setup();
            const string File = "File";
            var shortcut = setup.shell.SetupShellCreateShortcut(Combine(TestsBase.Path, Invariant($"{File}.lnk")));

            setup.creator.CreateShortcut(new DirectoryInfoWrapper(new DirectoryInfo(TestsBase.Path)), new FileInfo(File));

            shortcut.VerifySet(self => self.TargetPath = File, Once);
            var icon = Invariant($"{File}, 0");
            shortcut.VerifySet(self => self.IconLocation = icon, Once);
            shortcut.Verify(self => self.Save(), Once);
        }
#endregion

#region CreateShortcuts tests
        /// <summary><see cref="ShortcutCreator.CreateShortcuts" /> test.</summary>
        [Test]
        public static void CreateShortcuts_Called_DirectoriesCreated() {
            var setup = Setup();
            using var movie = CreateMovie();
            var languages = Empty<Language>();
            var subtitles = Empty<Language>();
            setup.builder.SetupBuilder(movie, languages, subtitles);
            setup.settings.SetupSettings();
            var cast = Combine(TestsBase.Path, "Cast", "Actor");
            const string File = "File.lnk";
            setup.shell.SetupShellCreateShortcut(Combine(cast, File));
            var directors = Combine(TestsBase.Path, "Directors", "Director");
            setup.shell.SetupShellCreateShortcut(Combine(directors, File));
            var genres = Combine(TestsBase.Path, "Genres", "Genre");
            setup.shell.SetupShellCreateShortcut(Combine(genres, File));
            var years = Combine(TestsBase.Path, "Years", "1");
            setup.shell.SetupShellCreateShortcut(Combine(years, File));

            setup.creator.CreateShortcuts(movie, languages, subtitles);

            setup.directory.Verify(self => self.CreateDirectory(cast), Once);
            setup.directory.Verify(self => self.CreateDirectory(directors), Once);
            setup.directory.Verify(self => self.CreateDirectory(genres), Once);
            setup.directory.Verify(self => self.CreateDirectory(years), Once);
        }

        /// <summary><see cref="ShortcutCreator.CreateShortcuts" /> test.</summary>
        [Test]
        public static void CreateShortcuts_Called_ShortcutsCreated() {
            var setup = Setup();
            using var movie = CreateMovie();
            var languages = Empty<Language>();
            var subtitles = Empty<Language>();
            setup.builder.SetupBuilder(movie, languages, subtitles);
            setup.settings.SetupSettings();
            const string File = "File";
            var link = Invariant($"{File}.lnk");
            var castShortcut = setup.shell.SetupShellCreateShortcut(Combine(TestsBase.Path, "Cast", "Actor", link));
            var directorsShortcut = setup.shell.SetupShellCreateShortcut(Combine(TestsBase.Path, "Directors", "Director", link));
            var genresShortcut = setup.shell.SetupShellCreateShortcut(Combine(TestsBase.Path, "Genres", "Genre", link));
            var yearsShortcut = setup.shell.SetupShellCreateShortcut(Combine(TestsBase.Path, "Years", "1", link));

            setup.creator.CreateShortcuts(movie, languages, subtitles);

            castShortcut.VerifySet(self => self.TargetPath = File, Once);
            var icon = Invariant($"{File}, 0");
            castShortcut.VerifySet(self => self.IconLocation = icon, Once);
            castShortcut.Verify(self => self.Save(), Once);
            directorsShortcut.VerifySet(self => self.TargetPath = File, Once);
            directorsShortcut.VerifySet(self => self.IconLocation = icon, Once);
            directorsShortcut.Verify(self => self.Save(), Once);
            genresShortcut.VerifySet(self => self.TargetPath = File, Once);
            genresShortcut.VerifySet(self => self.IconLocation = icon, Once);
            genresShortcut.Verify(self => self.Save(), Once);
            yearsShortcut.VerifySet(self => self.TargetPath = File, Once);
            yearsShortcut.VerifySet(self => self.IconLocation = icon, Once);
            yearsShortcut.Verify(self => self.Save(), Once);
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />s.</returns>
        static (ShortcutCreator creator, Mock<IFileNameBuilder> builder, Mock<IWshShell3> shell, Mock<IDirectory> directory, Mock<ISettings> settings) Setup() {
            var builder = new Mock<IFileNameBuilder>();
            var shell = new Mock<IWshShell3>();
            var directory = new Mock<IDirectory>();
            var settings = new Mock<ISettings>();
            return (new ShortcutCreator(builder.Object, shell.Object, directory.Object, settings.Object), builder, shell, directory, settings);
        }

        /// <summary>Sets up <paramref name="builder" />'s <see cref="IFileNameBuilder.BuildFileName" /> to return new file information.</summary>
        /// <param name="builder"><see cref="IFileNameBuilder" /> <see cref="Mock" /> to set up.</param>
        /// <param name="movie">Movie to expect.</param>
        /// <param name="languages">Languages to expect.</param>
        /// <param name="subtitles">Subtitles to expect.</param>
        static void SetupBuilder(this Mock<IFileNameBuilder> builder, Movie movie, IEnumerable<Language> languages, IReadOnlyCollection<Language> subtitles) =>
            builder.Setup(self => self.BuildFileName(movie, languages, subtitles)).Returns(new FileInfo("File"));

        /// <summary>Sets up <paramref name="settings" /> to return valid values.</summary>
        /// <param name="settings"><see cref="ISettings" /> <see cref="Mock" /> to set up.</param>
        static void SetupSettings(this Mock<ISettings> settings) {
            settings.SetupGet(self => self.Folders).Returns(new LibraryFolders("Cast", "Directors", "Genres", "Years"));
            settings.SetupGet(self => self.LibraryDirectory).Returns(new DirectoryInfoWrapper(new DirectoryInfo(TestsBase.Path)));
        }

        /// <summary>Sets up <paramref name="shell" />'s <see cref="IWshShell3.CreateShortcut" /> to return <see cref="IWshShortcut" /> <see cref="Mock" />.</summary>
        /// <param name="shell"><see cref="IWshShell3" /> <see cref="Mock" /> to set up.</param>
        /// <param name="pathLink">Path to expect.</param>
        /// <returns><see cref="IWshShortcut" /> <see cref="Mock" /> returned from <paramref name="shell" />'s <see cref="IWshShell3.CreateShortcut" />.</returns>
        static Mock<IWshShortcut> SetupShellCreateShortcut(this Mock<IWshShell3> shell, string pathLink) {
            var shortcut = new Mock<IWshShortcut>();
            shell.Setup(self => self.CreateShortcut(pathLink)).Returns(shortcut.Object);
            return shortcut;
        }
    }
}