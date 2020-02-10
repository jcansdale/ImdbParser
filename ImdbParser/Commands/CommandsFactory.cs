using ImdbParser.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static ImdbParser.Commands.AddCommand;
using static ImdbParser.Commands.CommandId;

namespace ImdbParser.Commands {
    /// <summary>Creates <see cref="ICommand" /> based on <see cref="CommandId" />.</summary>
    static class CommandsFactory {
        /// <returns>Created <see cref="ICommand" /> based on <paramref name="command" />.</returns>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="command" /> is <see cref="None" />.</exception>
        /// <exception cref="NotImplementedException">No <see cref="ICommand" /> creation is provided for <paramref name="command" />.</exception>
        internal static IResult<ICommand> CreateCommand(this CommandId command, IReadOnlyList<string> parameters, IReadOnlyCollection<IOption> options) =>
            command.ThrowIf(value => value == None, () => new InvalidEnumArgumentException(nameof(command), (int)None, typeof(CommandId))) == Add ? Create(parameters) : command == CommandId.Create ?
                Commands.CreateCommand.Create(parameters, options) : command == Extract ? ExtractCommand.Create(parameters) : command == Gather ? GatherCommand.Create(parameters, options) :
                    command == Remove ? (IResult<ICommand>)RemoveCommand.Create(parameters) : command == Rename ? RenameCommand.Create(parameters) : throw new NotImplementedException();
    }
}