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
    /// <summary>Extract command. Extracts tags from the movie.</summary>
    sealed class ExtractCommand : ICommand {
        /// <summary>Arguments that extract command accepts count.</summary>
        [DebuggerBrowsable(Never)] const int ArgumentsCount = 1;

        /// <summary>Initialize new <see cref="ExtractCommand" /> instance.</summary>
        /// <param name="moviePath">Path to a movie from which tags should be extracted.</param>
        ExtractCommand(FileInfo moviePath) => MoviePath = moviePath;

        /// <summary>Path to a movie from which tags should be extracted.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "False positive.")]
        internal FileInfo MoviePath { [DebuggerStepThrough] get; }

        /// <summary>Tries to create <see cref="ExtractCommand" /> instance. If any of the parameters passed are invalid then no result is returned.</summary>
        /// <param name="parameters">Direct command parameters.</param>
        /// <returns><see cref="ExtractCommand" /> instance if parameters are valid.</returns>
        internal static Result<ExtractCommand> Create(IReadOnlyList<string> parameters) {
            if (parameters.Count != ArgumentsCount) return Fail<ExtractCommand>(Invalid_command_parameters_count.CurrentCultureFormat(Extract, ArgumentsCount));
            var moviePath = new FileInfo(parameters[0]);
            return moviePath.Exists ? Ok(new ExtractCommand(moviePath)) : Fail<ExtractCommand>(File_does_not_exist);
        }
    }
}