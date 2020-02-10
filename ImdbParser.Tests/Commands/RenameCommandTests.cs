using ImdbParser.Commands;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Commands.RenameCommand;
using static ImdbParser.Messages;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.IO.Path;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="RenameCommand" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class RenameCommandTests {
#region Create tests
        /// <summary><see cref="RenameCommand.Create" /> test.</summary>
        [Test]
        public static void Create_ParametersContainsNoParameters_FailureReturned() => Create(Empty<string>()).ExpectFail(() => Invalid_command_parameters_count.InvariantCultureFormat(Rename, 2));

        /// <summary><see cref="RenameCommand.Create" /> test.</summary>
        [Test]
        public static void Create_NonExistingFilePassed_FailureReturned() => Create(new[] { string.Empty, Combine(Path, GetRandomFileName()) }).ExpectFail(() => File_does_not_exist);

        /// <summary><see cref="RenameCommand.Create" /> test.</summary>
        [Test]
        public static void Create_ValidParametersPassed_ParametersSets() {
            var currentFileName = GetRandomFileName();

            Create(new[] { currentFileName, Path }).ExpectSuccess(command => Multiple(() => {
                That(command.CurrentFileName.ToString, SameAs(currentFileName));
                That(command.NewFile.ToString, SameAs(Path));
            }));
        }
#endregion
    }
}