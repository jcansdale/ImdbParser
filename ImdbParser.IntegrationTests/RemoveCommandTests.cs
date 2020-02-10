using ImdbParser.Settings;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.IntegrationTests.TestsBase;
using static ImdbParser.Messages;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.FormattableString;
using static System.IO.File;
using static System.IO.Path;

namespace ImdbParser.IntegrationTests {
    /// <summary>Remove command tests.</summary>
    [ExcludeFromCodeCoverage]
    static class RemoveCommandTests {
#pragma warning disable CS3016
        /// <summary>Invalid parameters count.</summary>
        /// <param name="parameters">Parameters for <see cref="Remove" /> command.</param>
        /// <returns><see cref="Task" /> to execute.</returns>
        [TestCase, TestCase("", "")]
#pragma warning restore CS3016
        public static async Task InvalidParametersCount_FailureReturned(params string[] parameters) {
            var values = new List<string> { "-Remove" };
            values.AddRange(parameters);
            That((await ExecuteAsync(values.ToArray()).ConfigureAwait(default)).message, EqualTo(Invalid_command_parameters_count.CurrentCultureFormat(Remove, 1)));
        }

        /// <summary>Does not remove file in the root directory.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FileInTheRootDirectory_FileNotRemoved() {
            var settings = new TestSettings();
            var file = GetRandomFileName();
            var fullPath = Invariant($"{Combine(settings.LibraryDirectory.ToString(), file)}.lnk");
            Move(GetTempFileName(), fullPath);
            var executionInformation = await ExecuteAsync(collection => collection.AddSingleton<ISettings>(settings), "-Remove", file).ConfigureAwait(default);
            That(Exists(fullPath), Is.True, () => executionInformation.message);
        }

        /// <summary>Removes file in the inner directory.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task FileInInnerDirectory_FileAndDirectoryRemoved() {
            var settings = new TestSettings();
            var directory = Combine(settings.LibraryDirectory.ToString(), GetRandomFileName());
            var directoryInfo = Directory.CreateDirectory(directory);
            var file = GetRandomFileName();
            var fullPath = Invariant($"{Combine(directory, file)}.lnk");
            Move(GetTempFileName(), fullPath);
            var executionInformation = await ExecuteAsync(collection => collection.AddSingleton<ISettings>(settings), "-Remove", file).ConfigureAwait(default);
            Multiple(() => {
                That(Exists(fullPath), Is.False, () => executionInformation.message);
                That(directoryInfo.Exists, Is.False, () => executionInformation.message);
            });
        }

        /// <summary>Only required file renamed.</summary>
        /// <returns><see cref="Task" /> to execute.</returns>
        [Test]
        public static async Task HasMultipleFiles_OnlyRequiredFileRemoved() {
            var settings = new TestSettings();
            var directory = Combine(settings.LibraryDirectory.ToString(), GetRandomFileName());
            var directoryInfo = Directory.CreateDirectory(directory);
            var file1 = GetRandomFileName();
            var fullPath1 = Invariant($"{Combine(directory, file1)}.lnk");
            var fullPath2 = Combine(directory, GetRandomFileName());
            Move(GetTempFileName(), fullPath1);
            Move(GetTempFileName(), fullPath2);
            var executionInformation = await ExecuteAsync(collection => collection.AddSingleton<ISettings>(settings), "-Remove", file1).ConfigureAwait(default);
            Multiple(() => {
                That(Exists(fullPath1), Is.False, () => executionInformation.message);
                That(Exists(fullPath2), Is.True, () => executionInformation.message);
                That(directoryInfo.Exists, Is.True, () => executionInformation.message);
            });
        }
    }
}