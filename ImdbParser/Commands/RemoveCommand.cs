using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static ImdbParser.Commands.CommandId;
using static ImdbParser.Messages;
using static System.Diagnostics.DebuggerBrowsableState;
using static System.Result;

namespace ImdbParser.Commands {
    /// <summary>Remove command. Removes existing shortcuts.</summary>
    class RemoveCommand : ICommand {
        /// <summary>Arguments that remove command accepts count.</summary>
        [DebuggerBrowsable(Never)] const int ArgumentsCount = 1;

        /// <summary>Initialize new <see cref="RemoveCommand" /> instance.</summary>
        /// <param name="currentFileName">Current file name that has to be removed.</param>
        private protected RemoveCommand(FileInfo currentFileName) => CurrentFileName = currentFileName;

        /// <summary>Current file name that has to be renamed.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal FileInfo CurrentFileName { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="RemoveCommand" /> instance. If any of the parameters passed are invalid then no result is returned.</summary>
        /// <param name="parameters">Direct command parameters.</param>
        /// <returns><see cref="RemoveCommand" /> instance if parameters are valid.</returns>
        internal static Result<RemoveCommand> Create(IReadOnlyList<string> parameters) => parameters.Count == ArgumentsCount ? Ok(new RemoveCommand(new FileInfo(parameters[0]))) :
            Fail<RemoveCommand>(Invalid_command_parameters_count.CurrentCultureFormat(Remove, ArgumentsCount));
    }
}