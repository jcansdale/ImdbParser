using ImdbParser.Commands;
using System;
using System.Threading.Tasks;
using static System.Result;
using static System.String;
using static System.Threading.Tasks.Task;

namespace ImdbParser.Executors {
    /// <summary>Executes <see cref="ExtractCommand" />.</summary>
    sealed class ExtractExecutor : IExecutor<ExtractCommand> {
        /// <summary>MKV commands executor.</summary>
        readonly IMkvToolExecutor _executor;

        /// <summary>Initialize new <see cref="ExtractExecutor" /> instance.</summary>
        /// <param name="executor">MKV commands executor.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public ExtractExecutor(IMkvToolExecutor executor) => _executor = executor;

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        public Task<IResult<string>> ExecuteAsync(ExtractCommand command) =>
            _executor.ExtractTags(command.MoviePath).Map(result => FromResult<IResult<string>>(result.IsSuccess ? Ok(Empty) : Fail<string>(result)));
    }
}