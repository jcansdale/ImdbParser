using ImdbParser.Commands;
using ImdbParser.Options;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Messages;
using static ImdbParser.Tests.TestsBase;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;
using static System.Array;
using static System.Int32;
using static System.IO.Path;
using Throws = NUnit.Framework.Throws;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="CommandsFactory" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CommandsFactoryTests {
#region CreateCommand tests
        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_NoneType_ThrowsInvalidEnumArgumentException() => That(() => None.CreateCommand(default!, default!),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Matches<InvalidEnumArgumentException>(exception => exception.ParamName == "command"));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_UnknownType_ThrowsNotImplementedException() =>
            That(() => ((CommandId)MaxValue).CreateCommand(Empty<string>(), Empty<IOption>()), Throws.TypeOf<NotImplementedException>());

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_CreateFailed_FailureReturned() =>
            Add.CreateCommand(Empty<string>(), Empty<IOption>()).ExpectFail(() => Invalid_command_parameters_count.InvariantCultureFormat(Add, 1));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_AddCommand_AddCommandReturned() => Add.CreateCommand(new[] { Path }, Empty<IOption>()).ExpectSuccess(command => That(command, TypeOf(typeof(AddCommand))));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_CreateCommand_CreateCommandReturned() => Create.CreateCommand(new[] { ImdbTestUrl }, new IOption[] { CreateLanguage(), CreateLink() }).
            ExpectSuccess(command => That(command, TypeOf(typeof(CreateCommand))));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_ExtractCommand_ExtractCommandReturned() =>
            Extract.CreateCommand(new[] { Path }, Empty<IOption>()).ExpectSuccess(command => That(command, TypeOf(typeof(ExtractCommand))));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_GatherCommand_GatherCommandReturned() =>
            Gather.CreateCommand(new[] { ImdbTestUrl }, new[] { CreateLanguage() }).ExpectSuccess(command => That(command, TypeOf(typeof(GatherCommand))));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_RemoveCommand_RemoveCommandReturned() =>
            Remove.CreateCommand(new[] { GetRandomFileName() }, Empty<IOption>()).ExpectSuccess(command => That(command, TypeOf(typeof(RemoveCommand))));

        /// <summary><see cref="CommandsFactory.CreateCommand" /> test.</summary>
        [Test]
        public static void CreateCommand_RenameCommand_RenameCommandReturned() =>
            Rename.CreateCommand(new[] { GetRandomFileName(), Path }, Empty<IOption>()).ExpectSuccess(command => That(command, TypeOf(typeof(RenameCommand))));
#endregion
    }
}