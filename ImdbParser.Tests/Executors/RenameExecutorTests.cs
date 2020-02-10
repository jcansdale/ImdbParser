using ImdbParser.Executors;
using ImdbParser.Settings;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using static ImdbParser.Tests.TestsBase;
using static Moq.Times;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using static System.IO.Path;
using static System.IO.SearchOption;

namespace ImdbParser.Tests.Executors {
    /// <summary><see cref="RenameExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class RenameExecutorTests {
#region Constructor tests
        /// <summary><see cref="RenameExecutor(IShortcutCreator, ISettings)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentsPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new RenameExecutor(setup.creator.Object, setup.settings.Object), Not.Null);
        }
#endregion

#region ExecuteAsync tests
        /// <summary><see cref="RenameExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_NoDirectories_SuccessReturned() {
            var setup = Setup();
            setup.settings.SetupSettings();

            (await setup.executor.ExecuteAsync(CreateRename()).ConfigureAwait(default)).ExpectSuccess();
        }

        /// <summary><see cref="RenameExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_HasDirectoriesButNoFiles_SuccessReturned() {
            var setup = Setup();
            setup.settings.SetupSettings().SetupDirectoryEnumerateDirectories(new Mock<IDirectoryInfo>().Object);

            (await setup.executor.ExecuteAsync(CreateRename()).ConfigureAwait(default)).ExpectSuccess();
        }

        /// <summary><see cref="RenameExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FileFound_FileDeleted() {
            var setup = Setup();
            var subDirectory = new Mock<IDirectoryInfo>();
            setup.settings.SetupSettings().SetupDirectoryEnumerateDirectories(subDirectory.Object);
            var file = new Mock<IFileInfo>();
            subDirectory.SetupDirectoryEnumerateFiles(file.Object);
            var command = CreateRename();

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();

            file.Verify(self => self.Delete(), Once);
        }

        /// <summary><see cref="RenameExecutor.ExecuteAsync" /> test.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task ExecuteAsync_FileFound_NewShortcutCreated() {
            var setup = Setup();
            var subDirectory = new Mock<IDirectoryInfo>();
            setup.settings.SetupSettings().SetupDirectoryEnumerateDirectories(subDirectory.Object);
            subDirectory.SetupDirectoryEnumerateFiles(new Mock<IFileInfo>().Object);
            var command = CreateRename();

            (await setup.executor.ExecuteAsync(command).ConfigureAwait(default)).ExpectSuccess();

            setup.creator.Verify(self => self.CreateShortcut(subDirectory.Object, command.NewFile), Once);
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock" />s.</returns>
        static (RenameExecutor executor, Mock<IShortcutCreator> creator, Mock<ISettings> settings) Setup() {
            var creator = new Mock<IShortcutCreator>();
            var settings = new Mock<ISettings>();
            return (new RenameExecutor(creator.Object, settings.Object), creator, settings);
        }

        /// <summary>Sets up <paramref name="directory" /> <see cref="IDirectoryInfo.EnumerateDirectories" /> to return <paramref name="directories" />.</summary>
        /// <param name="directory"><see cref="IDirectoryInfo" /> <see cref="Mock" /> to set up.</param>
        /// <param name="directories">Directories to return from <paramref name="directory" /> <see cref="IDirectoryInfo.EnumerateDirectories" /></param>
        static void SetupDirectoryEnumerateDirectories(this Mock<IDirectoryInfo> directory, params IDirectoryInfo[] directories) =>
            directory.Setup(self => self.EnumerateDirectories("*", AllDirectories)).Returns(directories);

        /// <summary>Sets up <paramref name="directory" /> <see cref="IDirectoryInfo.EnumerateFiles" /> to return <paramref name="files" />.</summary>
        /// <param name="directory"><see cref="IDirectoryInfo" /> <see cref="Mock" /> to set up.</param>
        /// <param name="files">Files to return from <paramref name="directory" /> <see cref="IDirectoryInfo.EnumerateFiles" /></param>
        static void SetupDirectoryEnumerateFiles(this Mock<IDirectoryInfo> directory, params IFileInfo[] files) =>
            directory.Setup(self => self.EnumerateFiles(Invariant($"*{GetFileName(TestsBase.Path)}.lnk"))).Returns(files);

        /// <summary>Sets up <paramref name="settings" /> to return valid values.</summary>
        /// <param name="settings"><see cref="ISettings" /> <see cref="Mock" /> to set up.</param>
        /// <returns><see cref="IDirectoryInfo" /> <see cref="Mock" /> returned from <paramref name="settings" />'s <see cref="ISettings.LibraryDirectory" />.</returns>
        static Mock<IDirectoryInfo> SetupSettings(this Mock<ISettings> settings) {
            var directory = new Mock<IDirectoryInfo>();
            settings.SetupGet(self => self.LibraryDirectory).Returns(directory.Object);
            return directory;
        }
    }
}