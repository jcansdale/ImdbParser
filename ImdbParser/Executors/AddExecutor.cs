using ImdbParser.Commands;
using System;
using System.Threading.Tasks;
using static System.Result;
using static System.String;
using static System.Threading.Tasks.Task;

namespace ImdbParser.Executors {
    /// <summary>Executes <see cref="AddCommand" />.</summary>
    sealed class AddExecutor : IExecutor<AddCommand> {
        /// <summary>MKV commands executor.</summary>
        readonly IMkvToolExecutor _executor;

        /// <summary>Initialize new <see cref="AddExecutor" /> instance.</summary>
        /// <param name="executor">MKV commands executor.</param>
        /// <remarks>Must be public for dependency injection.</remarks>
        public AddExecutor(IMkvToolExecutor executor) => _executor = executor;

        /// <summary>Executes <paramref name="command" />.</summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Text to print after operation.</returns>
        public Task<IResult<string>> ExecuteAsync(AddCommand command) =>
            _executor.AddTags(command.MoviePath).Map(result => FromResult<IResult<string>>(result.IsSuccess ? Ok(Empty) : Fail<string>(result)));
    }
}