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
    /// <summary>Rename command. Renames existing shortcuts to the new one.</summary>
    sealed class RenameCommand : RemoveCommand {
        /// <summary>Arguments that rename command accepts count.</summary>
        [DebuggerBrowsable(Never)] const int ArgumentsCount = 2;

        /// <summary>Initialize new <see cref="RenameCommand" /> instance.</summary>
        /// <param name="currentFileName">Current file name that has to be renamed.</param>
        /// <param name="newFile">New file path.</param>
        RenameCommand(FileInfo currentFileName, FileInfo newFile) : base(currentFileName) => NewFile = newFile;

        /// <summary>New file path.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal FileInfo NewFile { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="RenameCommand" /> instance. If any of the parameters passed are invalid then no result is returned.</summary>
        /// <param name="parameters">Direct command parameters.</param>
        /// <returns><see cref="RenameCommand" /> instance if parameters are valid.</returns>
        internal static new Result<RenameCommand> Create(IReadOnlyList<string> parameters) {
            if (parameters.Count != ArgumentsCount) return Fail<RenameCommand>(Invalid_command_parameters_count.CurrentCultureFormat(Rename, ArgumentsCount));
            var newFile = new FileInfo(parameters[1]);
            return newFile.Exists ? Ok(new RenameCommand(new FileInfo(parameters[0]), newFile)) : Fail<RenameCommand>(File_does_not_exist);
        }
    }
}