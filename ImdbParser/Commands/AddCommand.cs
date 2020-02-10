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
    /// <summary>Add command. Adds tags to the movie.</summary>
    sealed class AddCommand : ICommand {
        /// <summary>Arguments that add command accepts count.</summary>
        [DebuggerBrowsable(Never)] const int ArgumentsCount = 1;

        /// <summary>Initialize new <see cref="AddCommand" /> instance.</summary>
        /// <param name="moviePath">Path to a movie to which tags should be added.</param>
        AddCommand(FileInfo moviePath) => MoviePath = moviePath;

        /// <summary>Path to a movie to which tags should be added.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal FileInfo MoviePath { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="AddCommand" /> instance. If any of the parameters passed are invalid then no result is returned.</summary>
        /// <param name="parameters">Direct command parameters.</param>
        /// <returns><see cref="AddCommand" /> instance if parameters are valid.</returns>
        internal static Result<AddCommand> Create(IReadOnlyList<string> parameters) {
            if (parameters.Count != ArgumentsCount) return Fail<AddCommand>(Invalid_command_parameters_count.CurrentCultureFormat(Add, ArgumentsCount));
            var moviePath = new FileInfo(parameters[0]);
            return moviePath.Exists ? Ok(new AddCommand(moviePath)) : Fail<AddCommand>(File_does_not_exist);
        }
    }
}