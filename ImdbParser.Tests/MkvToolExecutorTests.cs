using ImdbParser.Settings;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static ImdbParser.Messages;
using static Moq.It;
using static Moq.Times;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;

namespace ImdbParser.Tests {
    /// <summary><see cref="MkvToolExecutor" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class MkvToolExecutorTests {
#region Constructor tests
        /// <summary><see cref="MkvToolExecutor(IFile, IProcessStatic, IRegistry, ISettings)" /> test.</summary>
        [Test]
        public static void Constructor_ValidArgumentPassed_NoExceptionThrown() {
            var setup = Setup();

            That(new MkvToolExecutor(setup.file.Object, setup.process.Object, setup.registry.Object, setup.settings.Object), Not.Null);
        }
#endregion

#region AddTags tests
        /// <summary><see cref="MkvToolExecutor.AddTags" /> test.</summary>
        [Test]
        public static void AddTags_ToolRegistryEntryNotFound_FailureReturned() => Setup().executor.AddTags(new FileInfo(TestsBase.Path)).ExpectFail(() => MKV_Tool_Nix_is_not_installed);

        /// <summary><see cref="MkvToolExecutor.AddTags" /> test.</summary>
        [Test]
        public static void AddTags_ToolExecutableNotFound_FailureReturned() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();

            // ReSharper disable once StringLiteralTypo
            setup.executor.AddTags(new FileInfo(TestsBase.Path)).ExpectFail(() => MKV_Tool_Nix_executable_not_found.InvariantCultureFormat(@"C:\mkvpropedit.exe"));
        }

        /// <summary><see cref="MkvToolExecutor.AddTags" /> test.</summary>
        [Test]
        public static void AddTags_ToolExecutableFound_SuccessReturned() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();
            // ReSharper disable once StringLiteralTypo
            setup.file.SetupFileExists("mkvpropedit");
            setup.settings.SetupSettings();
            setup.process.SetupProcessStartForAdd();

            setup.executor.AddTags(new FileInfo(TestsBase.Path)).ExpectSuccess();
        }

        /// <summary><see cref="MkvToolExecutor.AddTags" /> test.</summary>
        [Test]
        public static void AddTags_ToolExecutableFound_ValidProcessCreated() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();
            // ReSharper disable once StringLiteralTypo
            setup.file.SetupFileExists("mkvpropedit");
            setup.settings.SetupSettings();
            setup.process.SetupProcessStartForAdd();

            setup.executor.AddTags(new FileInfo(TestsBase.Path)).ExpectSuccess();

            setup.process.Verify(self => self.Start(Is<ProcessStartInfo>(startInfo => startInfo.FileName == @"C:\mkvpropedit.exe" &&
                    startInfo.Arguments == Invariant($"\"{TestsBase.Path}\" --tags all:\"TagName\""))),
                Once);
        }

        /// <summary><see cref="MkvToolExecutor.AddTags" /> test.</summary>
        [Test]
        public static void AddTags_ToolExecutableFound_WaitedForProcessToExit() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();
            // ReSharper disable once StringLiteralTypo
            setup.file.SetupFileExists("mkvpropedit");
            setup.settings.SetupSettings();
            var process = setup.process.SetupProcessStartForAdd();

            setup.executor.AddTags(new FileInfo(TestsBase.Path)).ExpectSuccess();

            process.Verify(self => self.WaitForExit(), Once);
        }
#endregion

#region ExtractTags tests
        /// <summary><see cref="MkvToolExecutor.ExtractTags" /> test.</summary>
        [Test]
        public static void ExtractTags_ToolRegistryEntryNotFound_FailureReturned() => Setup().executor.ExtractTags(new FileInfo(TestsBase.Path)).ExpectFail(() => MKV_Tool_Nix_is_not_installed);

        /// <summary><see cref="MkvToolExecutor.ExtractTags" /> test.</summary>
        [Test]
        public static void ExtractTags_ToolExecutableNotFound_FailureReturned() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();

            // ReSharper disable once StringLiteralTypo
            setup.executor.ExtractTags(new FileInfo(TestsBase.Path)).ExpectFail(() => MKV_Tool_Nix_executable_not_found.InvariantCultureFormat(@"C:\mkvextract.exe"));
        }

        /// <summary><see cref="MkvToolExecutor.ExtractTags" /> test.</summary>
        [Test]
        public static void ExtractTags_ToolExecutableFound_SuccessReturned() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();
            // ReSharper disable once StringLiteralTypo
            setup.file.SetupFileExists("mkvextract");
            setup.settings.SetupSettings();
            setup.process.SetupProcessStartForExtract();

            setup.executor.ExtractTags(new FileInfo(TestsBase.Path)).ExpectSuccess();
        }

        /// <summary><see cref="MkvToolExecutor.ExtractTags" /> test.</summary>
        [Test]
        public static void ExtractTags_ToolExecutableFound_ValidProcessCreated() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();
            // ReSharper disable once StringLiteralTypo
            setup.file.SetupFileExists("mkvextract");
            setup.settings.SetupSettings();
            setup.process.SetupProcessStartForExtract();

            setup.executor.ExtractTags(new FileInfo(TestsBase.Path)).ExpectSuccess();

            setup.process.Verify(self => self.Start(Is<ProcessStartInfo>(startInfo => startInfo.FileName == @"C:\mkvextract.exe" &&
                    startInfo.Arguments == Invariant($"tags \"{TestsBase.Path}\" -r \"TagName\""))),
                Once);
        }

        /// <summary><see cref="MkvToolExecutor.ExtractTags" /> test.</summary>
        [Test]
        public static void ExtractTags_ToolExecutableFound_WaitedForProcessToExit() {
            var setup = Setup();
            setup.registry.SetupRegistryGetValue();
            // ReSharper disable once StringLiteralTypo
            setup.file.SetupFileExists("mkvextract");
            setup.settings.SetupSettings();
            var process = setup.process.SetupProcessStartForExtract();

            setup.executor.ExtractTags(new FileInfo(TestsBase.Path)).ExpectSuccess();

            process.Verify(self => self.WaitForExit(), Once);
        }
#endregion

        /// <summary>Single test setup.</summary>
        /// <returns><see cref="object" /> to test with its <see cref="Mock{T}" />.</returns>
        static (MkvToolExecutor executor, Mock<IFile> file, Mock<IProcessStatic> process, Mock<IRegistry> registry, Mock<ISettings> settings) Setup() {
            var file = new Mock<IFile>();
            var process = new Mock<IProcessStatic>();
            var registry = new Mock<IRegistry>();
            var settings = new Mock<ISettings>();
            return (new MkvToolExecutor(file.Object, process.Object, registry.Object, settings.Object), file, process, registry, settings);
        }

        /// <summary>Sets up <paramref name="file" />'s <see cref="IFile.Exists" /> to return true.</summary>
        /// <param name="file"><see cref="IFile" /> <see cref="Mock" /> to set up.</param>
        /// <param name="fileName">File name to expect.</param>
        // ReSharper disable once StringLiteralTypo
        static void SetupFileExists(this Mock<IFile> file, string fileName) => file.Setup(self => self.Exists(Invariant($@"C:\{fileName}.exe"))).Returns(true);

        /// <summary>Sets up <paramref name="process" />' <see cref="IProcessStatic.Start" /> to return <see cref="IProcess" /> <see cref="Mock" /> for
        /// <see cref="MkvToolExecutor.AddTags" />.</summary>
        /// <param name="process"><see cref="IProcessStatic" /> <see cref="Mock" /> to set up.</param>
        /// <returns><see cref="IProcess" /> <see cref="Mock" /> returned from <paramref name="process" />' <see cref="IProcessStatic.Start" />.</returns>
        static Mock<IProcess> SetupProcessStartForAdd(this Mock<IProcessStatic> process) {
            var processInstance = new Mock<IProcess>();
            process.Setup(self =>
                    self.Start(Is<ProcessStartInfo>(startInfo => startInfo.FileName == @"C:\mkvpropedit.exe" && startInfo.Arguments == Invariant($"\"{TestsBase.Path}\" --tags all:\"TagName\"")))).
                Returns(processInstance.Object);
            return processInstance;
        }

        /// <summary>Sets up <paramref name="process" />' <see cref="IProcessStatic.Start" /> to return <see cref="IProcess" /> <see cref="Mock" /> for
        /// <see cref="MkvToolExecutor.ExtractTags" />.</summary>
        /// <param name="process"><see cref="IProcessStatic" /> <see cref="Mock" /> to set up.</param>
        /// <returns><see cref="IProcess" /> <see cref="Mock" /> returned from <paramref name="process" />' <see cref="IProcessStatic.Start" />.</returns>
        static Mock<IProcess> SetupProcessStartForExtract(this Mock<IProcessStatic> process) {
            var processInstance = new Mock<IProcess>();
            process.Setup(self =>
                    self.Start(Is<ProcessStartInfo>(startInfo => startInfo.FileName == @"C:\mkvextract.exe" && startInfo.Arguments == Invariant($"tags \"{TestsBase.Path}\" -r \"TagName\"")))).
                Returns(processInstance.Object);
            return processInstance;
        }

        /// <summary>Sets up <paramref name="registry" /> <see cref="IRegistry.GetValue" /> to return a valid path.</summary>
        /// <param name="registry"><see cref="IRegistry" /> <see cref="Mock" /> to set up.</param>
        static void SetupRegistryGetValue(this Mock<IRegistry> registry) => registry.
            // ReSharper disable StringLiteralTypo
            Setup(self => self.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\mkvtoolnix-gui.exe", string.Empty, default)).Returns(@"C:\Value");
        // ReSharper restore StringLiteralTypo

        /// <summary>Setup <paramref name="settings" /> to return valid values.</summary>
        /// <param name="settings"><see cref="ISettings" /> <see cref="Mock" /> to set up.</param>
        static void SetupSettings(this Mock<ISettings> settings) => settings.SetupGet(self => self.TagsName).Returns(new FileInfo("TagName"));
    }
}