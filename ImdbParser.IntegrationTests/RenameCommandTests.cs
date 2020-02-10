using ImdbParser.Settings;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static ImdbParser.IntegrationTests.TestsBase;
using System.Threading.Tasks;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.IntegrationTests.AssemblySetup;
using static ImdbParser.Messages;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using static System.IO.File;
using static System.IO.Path;

namespace ImdbParser.IntegrationTests {
    /// <summary>Rename command tests.</summary>
    [ExcludeFromCodeCoverage]
    static class RenameCommandTests {
        /// <summary>File to use for links.</summary>
        // ReSharper disable once RedundantDefaultMemberInitializer
        static string _linkFile = default!;

        /// <summary>All tests setup.</summary>
        [OneTimeSetUp]
        public static void OneTimeSetup() {
            _linkFile = Combine(WorkingDirectory.ToString(), GetRandomFileName());
            Move(GetTempFileName(), _linkFile);
        }

#pragma warning disable CS3016
        /// <summary>Invalid parameters count.</summary>
        /// <param name="parameters">Parameters for <see cref="Rename" /> command.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        [TestCase, TestCase(""), TestCase("", "", "")]
#pragma warning restore CS3016
        public static async Task InvalidParametersCount_FailureReturned(params string[] parameters) {
            var values = new List<string> { "-Rename" };
            values.AddRange(parameters);
            That((await ExecuteAsync(values.ToArray()).ConfigureAwait(default)).message, EqualTo(Invalid_command_parameters_count.CurrentCultureFormat(Rename, 2)));
        }

        /// <summary>Parameter file not found.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task LinkNotFound_FailureReturned() =>
            That((await ExecuteAsync("-Rename", WorkingDirectory.ToString(), Combine(WorkingDirectory.ToString(), GetRandomFileName())).ConfigureAwait(default)).message, EqualTo(File_does_not_exist));

        /// <summary>Does not rename file in the root directory.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FileInTheRootDirectory_FileNotRenamed() {
            var settings = new TestSettings();
            var file = GetRandomFileName();
            var fullPath = Invariant($"{Combine(settings.LibraryDirectory.ToString(), file)}.lnk");
            Move(GetTempFileName(), fullPath);
            // ReSharper disable once ImplicitlyCapturedClosure
            var executionInformation = await ExecuteAsync(collection => collection.AddSingleton<ISettings>(settings), "-Rename", file, _linkFile).ConfigureAwait(default);
            Multiple(() => {
                That(Exists(fullPath), Is.True, () => executionInformation.message);
                That(Exists(Invariant($"{Combine(settings.LibraryDirectory.ToString(), GetFileName(_linkFile))}.lnk")), Is.False, () => executionInformation.message);
            });
        }

        /// <summary>Renames file in the inner directory.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FileInInnerDirectory_FileRenamed() {
            var settings = new TestSettings();
            var directory = Combine(settings.LibraryDirectory.ToString(), GetRandomFileName());
            Directory.CreateDirectory(directory);
            var file = GetRandomFileName();
            var fullPath = Invariant($"{Combine(directory, file)}.lnk");
            Move(GetTempFileName(), fullPath);
            var executionInformation = await ExecuteAsync(collection => collection.AddSingleton<ISettings>(settings), "-Rename", file, _linkFile).ConfigureAwait(default);
            Multiple(() => {
                That(Exists(fullPath), Is.False, () => executionInformation.message);
                That(Exists(Invariant($"{Combine(directory, GetFileName(_linkFile))}.lnk")), Is.True, () => executionInformation.message);
            });
        }

        /// <summary>Only required file renamed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task HasMultipleFiles_OnlyRequiredFileRenamed() {
            var settings = new TestSettings();
            var directory = Combine(settings.LibraryDirectory.ToString(), GetRandomFileName());
            Directory.CreateDirectory(directory);
            var file1 = GetRandomFileName();
            var fullPath1 = Invariant($"{Combine(directory, file1)}.lnk");
            var fullPath2 = Combine(directory, GetRandomFileName());
            Move(GetTempFileName(), fullPath1);
            Move(GetTempFileName(), fullPath2);
            var executionInformation = await ExecuteAsync(collection => collection.AddSingleton<ISettings>(settings), "-Rename", file1, _linkFile).ConfigureAwait(default);
            Multiple(() => {
                That(Exists(fullPath1), Is.False, () => executionInformation.message);
                That(Exists(fullPath2), Is.True, () => executionInformation.message);
                That(Exists(Invariant($"{Combine(directory, GetFileName(_linkFile))}.lnk")), Is.True, () => executionInformation.message);
            });
        }
    }
}