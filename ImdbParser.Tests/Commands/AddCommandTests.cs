using ImdbParser.Commands;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.AddCommand;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Messages;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.IO.Path;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="AddCommand" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class AddCommandTests {
#region Create tests
        /// <summary><see cref="AddCommand.Create" /> test.</summary>
        [Test]
        public static void Create_ParametersContainsNoParameters_FailureReturned() => Create(Empty<string>()).ExpectFail(() => Invalid_command_parameters_count.InvariantCultureFormat(Add, 1));

        /// <summary><see cref="RenameCommand.Create" /> test.</summary>
        [Test]
        public static void Create_NonExistingFilePassed_FailureReturned() => Create(new[] { Combine(Path, GetRandomFileName()) }).ExpectFail(() => File_does_not_exist);

        /// <summary><see cref="RenameCommand.Create" /> test.</summary>
        [Test]
        public static void Create_ValidParametersPassed_ParametersSets() => Create(new[] { Path }).ExpectSuccess(command => That(command.MoviePath.ToString, SameAs(Path)));
#endregion
    }
}