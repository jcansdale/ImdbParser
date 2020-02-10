using ImdbParser.Commands;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Commands.RemoveCommand;
using static ImdbParser.Messages;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.IO.Path;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="RemoveCommand" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class RemoveCommandTests {
#region Create tests
        /// <summary><see cref="RemoveCommand.Create" /> test.</summary>
        [Test]
        public static void Create_ParametersContainsNoParameters_FailureReturned() => Create(Empty<string>()).ExpectFail(() => Invalid_command_parameters_count.InvariantCultureFormat(Remove, 1));

        /// <summary><see cref="RenameCommand.Create" /> test.</summary>
        [Test]
        public static void Create_ValidParametersPassed_ParametersSets() {
            var currentFileName = GetRandomFileName();

            Create(new[] { currentFileName }).ExpectSuccess(command => That(command.CurrentFileName.ToString, SameAs(currentFileName)));
        }
#endregion
    }
}