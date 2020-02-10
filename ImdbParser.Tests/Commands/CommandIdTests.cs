using ImdbParser.Commands;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using static ImdbParser.Commands.CommandId;
using static NUnit.Framework.Assert;
using static NUnit.Framework.Is;

namespace ImdbParser.Tests.Commands {
    /// <summary><see cref="CommandId" /> tests.</summary>
    [ExcludeFromCodeCoverage]
    static class CommandIdTests {
#region Add tests
        /// <summary><see cref="Add" /> test.</summary>
        /// <param name="command">Command to convert to <see cref="string" />.</param>
        [TestCase(Add), TestCase(A)]
        public static void CommandId_AddToString_AddReturned(CommandId command) => That(command.ToString, EqualTo("Add"));
#endregion

#region Create tests
        /// <summary><see cref="Create" /> test.</summary>
        /// <param name="command">Command to convert to <see cref="string" />.</param>
        [TestCase(Create), TestCase(C)]
        public static void CommandId_CreateToString_CreateReturned(CommandId command) => That(command.ToString, EqualTo("Create"));
#endregion

#region Extract tests
        /// <summary><see cref="Extract" /> test.</summary>
        /// <param name="command">Command to convert to <see cref="string" />.</param>
        [TestCase(Extract), TestCase(E)]
        public static void CommandId_ExtractToString_ExtractReturned(CommandId command) => That(command.ToString, EqualTo("Extract"));
#endregion

#region Gather tests
        /// <summary><see cref="Gather" /> test.</summary>
        /// <param name="command">Command to convert to <see cref="string" />.</param>
        [TestCase(Gather), TestCase(G)]
        public static void CommandId_GatherToString_GatherReturned(CommandId command) => That(command.ToString, EqualTo("Gather"));
#endregion

#region Remove tests
        /// <summary><see cref="Remove" /> test.</summary>
        /// <param name="command">Command to convert to <see cref="string" />.</param>
        [TestCase(Remove), TestCase(Rem)]
        public static void CommandId_RemoveToString_RemoveReturned(CommandId command) => That(command.ToString, EqualTo("Remove"));
#endregion

#region Rename tests
        /// <summary><see cref="Rename" /> test.</summary>
        /// <param name="command">Command to convert to <see cref="string" />.</param>
        [TestCase(Rename), TestCase(Ren)]
        public static void CommandId_RenameToString_RenameReturned(CommandId command) => That(command.ToString, EqualTo("Rename"));
#endregion
    }
}